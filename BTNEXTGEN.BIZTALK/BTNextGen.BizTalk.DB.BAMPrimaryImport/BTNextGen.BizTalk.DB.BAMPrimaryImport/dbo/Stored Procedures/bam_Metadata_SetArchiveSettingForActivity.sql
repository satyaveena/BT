CREATE PROCEDURE [dbo].[bam_Metadata_SetArchiveSettingForActivity]
(
    @activityName NVARCHAR(128),
    @archiveSetting TINYINT
)
AS
    UPDATE [dbo].[bam_Metadata_Activities] 
    SET Archive = @archiveSetting
    WHERE ActivityName = @activityName