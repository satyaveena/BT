CREATE PROCEDURE [dbo].[bam_Metadata_SetConfigurationXml]
(
    @configXml    NTEXT,
    @loginName  NVARCHAR(256)
)
AS
    INSERT [bam_Metadata_Configuration] (ConfigurationXml, LastUpdated, LastUpdatedBy) 
    VALUES (@configXml, GETUTCDATE(), @loginName)