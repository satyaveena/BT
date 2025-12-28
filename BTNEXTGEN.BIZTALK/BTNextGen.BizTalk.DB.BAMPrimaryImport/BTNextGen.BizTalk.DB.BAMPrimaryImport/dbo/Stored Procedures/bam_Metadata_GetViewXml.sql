CREATE PROCEDURE [dbo].[bam_Metadata_GetViewXml]
AS
    SELECT  ViewName, DefinitionXml FROM dbo.bam_Metadata_Views
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetViewXml] TO [BAM_ManagementWS]
    AS [dbo];

