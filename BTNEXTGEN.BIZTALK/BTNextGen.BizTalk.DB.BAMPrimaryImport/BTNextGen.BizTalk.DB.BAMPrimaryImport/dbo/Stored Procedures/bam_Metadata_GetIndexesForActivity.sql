CREATE PROCEDURE [dbo].[bam_Metadata_GetIndexesForActivity]
(
    @activityName NVARCHAR(128)
)
AS
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    SELECT  IndexName, ColumnsList
    FROM    [dbo].[bam_Metadata_CustomIndexes]
    WHERE   ActivityName = @activityName