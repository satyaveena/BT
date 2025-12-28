CREATE PROCEDURE [dbo].[bam_Metadata_CreateIndex]
(
    @activityName NVARCHAR(128),
    @indexName NVARCHAR(128),
    @checkPoints NVARCHAR(2000),
    @updatedActivityDefinitionXml NTEXT
)
AS
    DECLARE @return_status INT

    exec @return_status = [dbo].[bam_Metadata_CreateCustomIndexOnly] @activityName = @activityName, @indexName = @indexName, @checkPoints = @checkPoints
    IF (@return_status <> 0)
        RETURN @return_status

    -- Update the activity definition Xml to include the new index
    UPDATE [dbo].[bam_Metadata_Activities]
    SET DefinitionXml = @updatedActivityDefinitionXml
    WHERE ActivityName = @activityName