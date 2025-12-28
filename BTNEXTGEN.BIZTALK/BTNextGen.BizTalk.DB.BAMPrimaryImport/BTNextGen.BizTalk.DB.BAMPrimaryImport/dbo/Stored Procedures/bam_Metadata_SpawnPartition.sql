CREATE PROCEDURE [dbo].[bam_Metadata_SpawnPartition]
(
    @activityName NVARCHAR(256)
)
AS
    BEGIN TRAN

    -- Pause the primary import until this SP is executed
    SELECT ActivityName FROM [dbo].[bam_Metadata_Activities] WITH (ROWLOCK, XLOCK) WHERE ActivityName = @activityName

    -- If previous archiving run is still in progress, continue last run
    SELECT ActivityName FROM [dbo].[bam_Metadata_Partitions]
    WHERE ArchivingInProgress = 1 AND ActivityName = @activityName
    IF @@ROWCOUNT > 0
    BEGIN
        COMMIT
        RETURN 255
    END

    /*
     * Step 1. Validate current activity's qualification for a new paritition
     *
     *    a. Completed instance table exists
     *    b. # of partition tables less than 254
     */

    DECLARE @completedInstancesTable sysname
    DECLARE @completedRelationshipsTable sysname
    SET @completedInstancesTable = N'[dbo].[bam_' + @activityName + N'_Completed]'
    SET @completedRelationshipsTable = N'[dbo].[bam_' + @activityName + N'_CompletedRelationships]'

    -- Check if current activity qualifies for a new partition
    DECLARE @MaxRecordID BIGINT
    DECLARE @MinRecordID BIGINT
    
    -- Get completed instance table's record ID range
    CREATE TABLE #RecordRange(MaxRecordID BIGINT, MinRecordID BIGINT)
    EXEC ('INSERT #RecordRange SELECT MAX(RecordID), MIN(RecordID) FROM ' + @completedInstancesTable)
    SELECT TOP 1 @MaxRecordID = MaxRecordID, @MinRecordID = MinRecordID FROM #RecordRange
    
    IF( @MaxRecordID IS NULL )
    BEGIN
        -- If the completed table is empty, don't create new partitions.
        COMMIT TRAN
        RETURN 0
    END

    -- Get total # of online partitions (excluding completed instance table) of current activity
    DECLARE @PartitionCount INT
    SELECT @PartitionCount = COUNT(*) FROM [dbo].[bam_Metadata_Partitions] WHERE ActivityName = @activityName AND ArchivedTime IS NULL
    IF (@PartitionCount >= 253)
    BEGIN
        -- If already have 253 partitions, COMMIT TRAN and return
        COMMIT TRAN
        RETURN 0
    END

    /*
     * Step 2. Swap out completed instances and relationships tables
     */

    -- Get ID and name of the trigger on completed instances table
    DECLARE @rtaTriggerName sysname
    SELECT @rtaTriggerName = name FROM sysobjects 
    WHERE parent_obj = OBJECT_ID(@completedInstancesTable) 
        AND name LIKE N'bam_%_RTACompletedTrigger'
        
    -- Create new unique partition table and relationship table names
    DECLARE @PartitionID uniqueidentifier
    DECLARE @PartitionName sysname
    DECLARE @instancePartitionTable sysname
    DECLARE @instancePartitionTableName sysname
    DECLARE @relationshipPartitionTable sysname
    DECLARE @relationshipPartitionTableName sysname

    SET @PartitionID = newid()
    SET @PartitionName = REPLACE(CONVERT(NVARCHAR(40), @PartitionID), '-', '_')

    SET @instancePartitionTableName = N'bam_' + @activityName + N'_' + @PartitionName
    SET @instancePartitionTable = N'[dbo].[' + @instancePartitionTableName + N']'

    SET @relationshipPartitionTableName = N'bam_' + @activityName + N'_' + @PartitionName + N'_Relationships'
    SET @relationshipPartitionTable = N'[dbo].[' + @relationshipPartitionTableName + ']'

    -- Rename the completed relationships table
    DECLARE @result INT
    DECLARE @oldTable sysname
    SET @oldTable = @completedInstancesTable
    EXEC @result = sp_rename @oldTable, @instancePartitionTableName, N'object'
    IF (@result <> 0)
    BEGIN
        RAISERROR (N'SpawnPartition_FailToRenameCompletedInstanceTable', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK 
        RETURN
    END

    -- Re-create the completed instance table schema
    EXEC ('SELECT TOP 1 * INTO ' + @completedInstancesTable + ' FROM ' + @instancePartitionTable 
        + ' ORDER BY RecordID DESC; DELETE FROM ' + @completedInstancesTable)
    IF (@@ERROR <> 0)
    BEGIN
        RAISERROR (N'SpawnPartition_FailToCreateCompletedInstanceTable', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    -- Re-create the RTA trigger on the new completed instance table
    EXEC [dbo].[bam_Metadata_RecreateRtaTriggerOnCompletedTable] @activityName, @rtaTriggerName

    -- Re-create the custom indexes on the new completed instances table
    EXEC @result = [dbo].[bam_Metadata_CreatePartitionIndexes] @activityName, @completedInstancesTable
    IF (@result <> 0)
    BEGIN
        RAISERROR (N'SpawnPartition_FailToCreateIndexOnInstancePartition', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    -- Rename the completed relationships table
    SET @oldTable = @completedRelationshipsTable
    EXEC @result = sp_rename @oldTable, @relationshipPartitionTableName, N'object'
    IF (@result <> 0)
    BEGIN
        RAISERROR (N'SpawnPartition_FailToRenameCompletedRelationshipTable', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    -- Re-create the completed relationships table schema
    EXEC ('SELECT TOP 0 * INTO ' + @completedRelationshipsTable + ' FROM ' + @relationshipPartitionTable)
    IF (@@ERROR <> 0)
    BEGIN
        RAISERROR (N'SpawnPartition_FailToCreateCompletedRelationshipTable', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    -- Rebuild archiving instance and relationship views
    EXEC ('ALTER VIEW [dbo].[bam_' + @activityName 
        + '_InstancesForArchive] AS SELECT TOP 0 * FROM ' + @completedInstancesTable + ' WITH (NOLOCK)')
    IF (@@ERROR <> 0)
    BEGIN
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    EXEC ('ALTER VIEW dbo.[bam_' + @activityName + 
        '_RelationshipsForArchive] AS SELECT TOP 0 * FROM ' + @completedRelationshipsTable + ' WITH (NOLOCK)')
    IF (@@ERROR <> 0)
    BEGIN
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    -- Re-create the custom indexes on the new completed relationships table
    DECLARE @@RelIndexName NVARCHAR(50)
    SET @@RelIndexName = 'RelPK_' + replace(CONVERT(NVARCHAR(40), NEWID()), '-', '_')
    EXEC ('CREATE CLUSTERED INDEX PK_' + @@RelIndexName + ' ON ' + @completedRelationshipsTable
        + '(RecordID, ReferenceName, ReferenceData)')
    IF (@@ERROR <> 0)
    BEGIN
        RAISERROR (N'SpawnPartition_FailToCreateIndexOnRelationshipPartition', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END
    EXEC ('CREATE NONCLUSTERED INDEX NCI_ActivityID ON ' + @completedRelationshipsTable + ' (ActivityID)')
    IF (@@ERROR <> 0)
    BEGIN
        RAISERROR (N'SpawnPartition_FailToCreateIndexOnRelationshipPartition', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    DECLARE @constraintName nvarchar(100)
    SET @constraintName = '[constraint_referenceType_' + replace(CONVERT(NVARCHAR(40), NEWID()), '-', '_') + ']'
    EXEC ('ALTER TABLE ' + @completedRelationshipsTable + ' WITH NOCHECK ADD CONSTRAINT ' 
        + @constraintName + ' DEFAULT N''BizTalkService'' FOR ReferenceType WITH VALUES')
    
    /* 
     * Step 3. Register the new completed tables and new partitions in the meta-data table
     */
     
    -- Register the new partitions
     INSERT [dbo].[bam_Metadata_Partitions](
        ActivityName, 
        InstancesTable,
         RelationshipsTable, 
        CreationTime, 
        MinRecordID, 
        MaxRecordID)
    VALUES (
        @activityName, 
        @instancePartitionTableName,
        @relationshipPartitionTableName, 
        GETUTCDATE(), 
        @MinRecordID, 
        @MaxRecordID)
    IF (@@ERROR <> 0 OR @@ROWCOUNT = 0)
    BEGIN
        RAISERROR (N'SpawnPartition_FailToRegisterPartition', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    /* 
     * Step 4. Reconstruct the completed instances and relationships views
     */
     
    EXEC @result = [dbo].[bam_Metadata_RegenerateViews] @activityName
    IF (@result <> 0)
    BEGIN
        PRINT 'Error: fail to regenerate views.'
        RAISERROR (N'SpawnPartition_FailToRegenerateViews', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    PRINT 'Partition ' + @instancePartitionTable + ' Created successfully (MinRecordID: ' 
        + CAST(@MinRecordID AS NVARCHAR) + ', MaxRecordID: ' + CAST(@MaxRecordID AS NVARCHAR) + ').'
        
    COMMIT TRAN
    
    RETURN 0