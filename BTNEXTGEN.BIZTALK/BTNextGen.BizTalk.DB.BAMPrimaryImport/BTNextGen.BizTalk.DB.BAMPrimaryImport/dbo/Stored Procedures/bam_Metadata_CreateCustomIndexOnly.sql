CREATE PROCEDURE [dbo].[bam_Metadata_CreateCustomIndexOnly]
(
    @activityName NVARCHAR(128),
    @indexName NVARCHAR(128),
    @checkPoints NVARCHAR(2000)
)
AS
    -- Internal only SPROC, don't run directly
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    DECLARE @createIndexSql NVARCHAR(4000)
    DECLARE @sqlStatementPart1 NVARCHAR(500)
    DECLARE @sqlStatementPart2 NVARCHAR(3200)
    SET @sqlStatementPart1 = N'CREATE NONCLUSTERED INDEX [NCI_' + @indexName + N'] ON '
    SET @sqlStatementPart2 = N' (' + @checkPoints + N')'

    DECLARE @instancePartition NVARCHAR(128)

    DECLARE @return_status INT

    -- Create Index on the partitions
    DECLARE partitions CURSOR LOCAL FOR 
        SELECT    InstancesTable
        FROM    [dbo].[bam_Metadata_Partitions] 
        WHERE    ActivityName = @activityName
            AND ArchivedTime IS NULL

    OPEN partitions
    FETCH NEXT FROM partitions INTO @instancePartition
    WHILE @@fetch_status = 0
    BEGIN
        SET @createIndexSql = @sqlStatementPart1 + @instancePartition + @sqlStatementPart2
        EXEC @return_status = sp_executesql @createIndexSql

        IF (@return_status <> 0)
            RETURN @return_status

        FETCH NEXT FROM partitions INTO @instancePartition
    END

    CLOSE partitions
    DEALLOCATE partitions

    -- Create Index on the completed instance table
    SET @createIndexSql = @sqlStatementPart1 + N'[dbo].[bam_' + @activityName + '_Completed]' + @sqlStatementPart2
    EXEC @return_status = sp_executesql @createIndexSql

    IF (@return_status <> 0)
        RETURN @return_status

    -- And now insert the new index definition into the CustomIndexes table
    INSERT INTO [dbo].[bam_Metadata_CustomIndexes]
    VALUES (
        @activityName,
        @indexName,
        @checkPoints
    )