CREATE PROCEDURE [dbo].[bam_Metadata_EndArchiving]
(
    @activityName NVARCHAR(256)
)
AS
    BEGIN TRAN

    -- Pause the primary import until current Operation finishes execution
    DECLARE @@activityName NVARCHAR(256)
    
    SELECT @@activityName = ActivityName 
    FROM [dbo].[bam_Metadata_Activities] WITH (ROWLOCK, XLOCK) 
    WHERE ActivityName = @activityName
    
    IF (@@ERROR <> 0)
    BEGIN
        RAISERROR (N'EndArchiving_FailToPausePrimaryImport', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END
    
    -- Check if any partition is currently being archived, if not just commit and return
    SELECT ActivityName FROM [dbo].[bam_Metadata_Partitions]
    WHERE ActivityName = @activityName AND ArchivingInProgress = 1 AND ArchivedTime IS NULL
    
    IF (@@ROWCOUNT = 0)
    BEGIN
        COMMIT TRAN
        RETURN 0
    END
    
    -- Rebuild activity's archiving instance and relationship views
    EXEC ('ALTER VIEW dbo.[bam_' + @activityName + '_InstancesForArchive] AS SELECT TOP 0 * FROM dbo.[bam_' 
        + @activityName + '_Completed]')
    IF (@@ERROR <> 0)
    BEGIN
        RAISERROR (N'EndArchiving_FailToResetInstanceArchivingView', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    EXEC ('ALTER VIEW dbo.[bam_' + @activityName + '_RelationshipsForArchive] AS SELECT TOP 0 * FROM dbo.[bam_' 
        + @activityName + '_CompletedRelationships]')
    IF (@@ERROR <> 0)
    BEGIN
        RAISERROR (N'EndArchiving_FailToCreateResetArchivingView', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    DECLARE @@instancesTable NVARCHAR(256)
    DECLARE @@RelationshipsTable NVARCHAR(256)
    
    -- Now we can safely drop the partitions which have already been archived
    DECLARE partition_cursor CURSOR local 
    FOR SELECT InstancesTable, RelationshipsTable 
        FROM [dbo].[bam_Metadata_Partitions] 
        WHERE ActivityName = @activityName AND ArchivingInProgress = 1 AND ArchivedTime IS NULL

    OPEN partition_cursor
    FETCH NEXT FROM partition_cursor INTO @@instancesTable, @@RelationshipsTable
    WHILE @@fetch_status = 0 
    BEGIN
        EXEC ('DROP TABLE [dbo].[' + @@instancesTable + ']')
        EXEC ('DROP TABLE [dbo].[' + @@RelationshipsTable + ']')
        PRINT 'Partition ' + @@instancesTable + ' was deleted'
        
        FETCH NEXT FROM partition_cursor INTO @@instancesTable, @@RelationshipsTable
    END
    CLOSE partition_cursor
    DEALLOCATE partition_cursor

    -- Reset archiving bit to zero and log archived time
    UPDATE [dbo].[bam_Metadata_Partitions]
    SET ArchivingInProgress = 0, ArchivedTime = GETUTCDATE()
    WHERE ActivityName = @activityName AND ArchivingInProgress = 1 AND ArchivedTime IS NULL
    
    -- Rebuild the completed instances and relationship views
    DECLARE @@result int
    EXEC @@result = [dbo].[bam_Metadata_RegenerateViews] @activityName
    IF (@@result <> 0)
    BEGIN
        RAISERROR (N'EndArchiving_FailToRegenerateViews', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN
    END

    COMMIT TRAN
    RETURN 0