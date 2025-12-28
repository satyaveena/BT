-- Create clustered and non-clustered indexes on specified table
-- Called by bam_Metadata_SpawnPartition
CREATE PROCEDURE [dbo].[bam_Metadata_CreatePartitionIndexes]
(
    @activityName NVARCHAR(256), 
    @instancesTable NVARCHAR(256)
)
AS
    DECLARE @@IndexName sysname
    DECLARE @@Columns NVARCHAR(4000)
    
    -- Always create clustered index on RecordID for partition table
    SET @@IndexName = 'PK_' + CONVERT(NVARCHAR(40), newid())
    SET @@IndexName = replace(@@IndexName, '-', '_')
    EXEC ('CREATE CLUSTERED INDEX ' + @@IndexName + ' ON ' + @instancesTable + ' (RecordID)')
    IF (@@ERROR <> 0) RETURN

    -- Always create non-clustered index on ActivityID for partition table
    EXEC ('CREATE UNIQUE NONCLUSTERED INDEX NCI_ActivityID ON ' + @instancesTable + '(ActivityID)')
    IF (@@ERROR <> 0) RETURN

    -- Always create non-clustered index on ActivityID for partition table
    EXEC ('CREATE NONCLUSTERED INDEX NCI_LastModified ON ' + @instancesTable + '(LastModified)')
    IF (@@ERROR <> 0) RETURN

    -- Create custom index
    DECLARE index_cursor CURSOR local 
    FOR SELECT IndexName, ColumnsList FROM [dbo].[bam_Metadata_CustomIndexes] WHERE ActivityName = @activityName
    
    OPEN index_cursor
    FETCH NEXT FROM index_cursor INTO @@IndexName, @@Columns
    
    WHILE @@fetch_status = 0 
    BEGIN
        -- CREATE the index
        EXEC ('CREATE NONCLUSTERED INDEX [NCI_' + @@IndexName + '] ON ' + @instancesTable + '(' + @@Columns + ')')
        IF (@@ERROR <> 0) RETURN
        FETCH NEXT FROM index_cursor INTO @@IndexName, @@Columns
    END
    
    CLOSE index_cursor
    DEALLOCATE index_cursor