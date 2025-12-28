CREATE PROCEDURE [dbo].[bam_Metadata_UpdateActivity]
(
    @activityName NVARCHAR(128),
    @definitionXml NTEXT
)
AS
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    -- Update activity in the metadata table
    UPDATE [dbo].[bam_Metadata_Activities]
    SET DefinitionXml = @definitionXml
    WHERE ActivityName = @activityName