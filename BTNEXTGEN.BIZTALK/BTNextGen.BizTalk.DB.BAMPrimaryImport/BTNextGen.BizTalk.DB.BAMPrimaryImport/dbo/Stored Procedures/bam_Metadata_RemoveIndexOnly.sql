CREATE PROCEDURE [dbo].[bam_Metadata_RemoveIndexOnly]
(
    @activityName NVARCHAR(128),
    @indexName NVARCHAR(128)
)
AS
    -- Internal ony stored proc, don't run directly 
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    DECLARE @removeIndexSql NVARCHAR(4000)
    DECLARE @sqlStatementPart1 NVARCHAR(500)
    DECLARE @sqlStatementPart2 NVARCHAR(500)
    DECLARE @sqlStatementPart3 NVARCHAR(500)
    SET @sqlStatementPart1 = N'IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N''[dbo].['
    SET @sqlStatementPart2 = N']'') AND name = N''NCI_' + @indexName + N''') ' 
    SET @sqlStatementPart3 = N'DROP INDEX ' 

    DECLARE @instancePartition NVARCHAR(128)

    DECLARE @return_status INT

    -- Remove index from the partitions
    DECLARE partitions CURSOR LOCAL FOR 
        SELECT    InstancesTable
        FROM    [dbo].[bam_Metadata_Partitions] 
        WHERE    ActivityName = @activityName
            AND ArchivedTime IS NULL

    OPEN partitions
    FETCH NEXT FROM partitions INTO @instancePartition
    WHILE @@fetch_status = 0
    BEGIN
        SET @removeIndexSql = @sqlStatementPart1 + @instancePartition + @sqlStatementPart2 + @sqlStatementPart3 + @instancePartition + N'.[NCI_' + @indexName + N']'
        EXEC @return_status = sp_executesql @removeIndexSql

        IF (@return_status <> 0)
            RETURN @return_status

        FETCH NEXT FROM partitions INTO @instancePartition
    END

    CLOSE partitions
    DEALLOCATE partitions

    -- Remove index from the completed instance table
    SET @removeIndexSql = @sqlStatementPart1 + N'bam_' + @activityName + '_Completed' + @sqlStatementPart2 + @sqlStatementPart3 + N'[dbo].[bam_' + @activityName + '_Completed]' + N'.[NCI_' + @indexName + N']'
    EXEC @return_status = sp_executesql @removeIndexSql

    IF (@return_status <> 0)
        RETURN @return_status

    -- And now remove the new index from the CustomIndexes table
    DELETE FROM [dbo].[bam_Metadata_CustomIndexes]
    WHERE ActivityName = @activityName
        AND IndexName = @indexName