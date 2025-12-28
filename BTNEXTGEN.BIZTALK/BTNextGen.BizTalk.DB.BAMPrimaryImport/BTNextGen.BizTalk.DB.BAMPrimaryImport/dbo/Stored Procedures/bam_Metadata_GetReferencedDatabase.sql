CREATE PROCEDURE [dbo].[bam_Metadata_GetReferencedDatabase]
AS
    SELECT ServerName, DatabaseName FROM dbo.bam_Metadata_ReferencedDatabases
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetReferencedDatabase] TO [BAM_ManagementWS]
    AS [dbo];

