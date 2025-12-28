CREATE PROCEDURE [dbo].[bam_Metadata_RemoveIndex]
(
    @activityName NVARCHAR(128),
    @indexName NVARCHAR(128),
    @updatedActivityDefinitionXml NTEXT
)
AS
    DECLARE @return_status INT

    EXEC @return_status = [dbo].[bam_Metadata_RemoveIndexOnly] @activityName = @activityName, @indexName = @indexName 
    IF (@return_status <> 0)
        RETURN @return_status

    -- Update the activity definition Xml to exclude the index
    UPDATE [dbo].[bam_Metadata_Activities]
    SET DefinitionXml = @updatedActivityDefinitionXml
    WHERE ActivityName = @activityName