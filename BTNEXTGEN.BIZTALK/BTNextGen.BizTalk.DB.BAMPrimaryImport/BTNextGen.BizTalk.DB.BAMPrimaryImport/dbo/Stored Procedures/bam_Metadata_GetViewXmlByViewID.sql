CREATE PROCEDURE [dbo].[bam_Metadata_GetViewXmlByViewID]
(
    @viewID sysname
)
AS
    SELECT ViewName, DefinitionXml FROM dbo.bam_Metadata_Views
    WHERE ViewID = @viewID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetViewXmlByViewID] TO [BAM_ManagementWS]
    AS [dbo];

