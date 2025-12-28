CREATE PROCEDURE [dbo].[bam_Metadata_SetActivityWindow]
(
    @activityName NVARCHAR(128),
    @timeLength INT,
    @timeUnit CHAR(10)
)
AS
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    UPDATE  [dbo].[bam_Metadata_Activities]
    SET     OnlineWindowTimeLength = @timeLength,
            OnlineWindowTimeUnit = @timeUnit
    WHERE ActivityName = @activityName