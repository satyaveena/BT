CREATE PROCEDURE [dbo].[bam_Metadata_GetArchiveSettingForActivity]
(
    @activityName NVARCHAR(128)
)
AS
    SELECT Archive 
    FROM [dbo].[bam_Metadata_Activities] 
    WHERE ActivityName = @activityName