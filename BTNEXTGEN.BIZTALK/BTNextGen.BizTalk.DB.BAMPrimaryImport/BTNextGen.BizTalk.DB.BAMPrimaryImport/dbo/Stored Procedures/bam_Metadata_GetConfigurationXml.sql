CREATE PROCEDURE [dbo].[bam_Metadata_GetConfigurationXml]
AS
    SELECT TOP 1 ConfigurationXml from [dbo].[bam_Metadata_Configuration] ORDER BY LastUpdated DESC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetConfigurationXml] TO [BAM_ManagementWS]
    AS [dbo];

