CREATE PROCEDURE [dbo].[bam_Metadata_DropClusteredIndex]
(
    @tableName     sysname
)
AS
    -- Internal ony stored proc, don't run directly 
    DECLARE @@indexName sysname
    DECLARE indexNames CURSOR FOR
        SELECT name FROM sys.indexes
        WHERE object_id = OBJECT_ID(@tableName)	AND type=1

    OPEN indexNames
    FETCH NEXT FROM indexNames INTO @@indexName
    WHILE @@fetch_status = 0
    BEGIN
        EXEC(N'DROP INDEX '+ @tableName + '.['+ @@indexName + N']')
        FETCH NEXT FROM indexNames INTO @@indexName
    END
    CLOSE indexNames
    DEALLOCATE indexNames