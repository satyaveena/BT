CREATE PROCEDURE [dbo].[bam_Metadata_GetActivityWindow]
(
    @activityName NVARCHAR(128)
)
AS
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    SELECT OnlineWindowTimeLength, OnlineWindowTimeUnit
    FROM [dbo].[bam_Metadata_Activities]
    WHERE ActivityName = @activityName