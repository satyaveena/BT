CREATE PROCEDURE [dbo].[bam_Metadata_GetDatabaseVersion]
AS
        SELECT TOP 1
               MajorVersion,
               MinorVersion,
               BuildVersion,
               RevisionVersion,
               SKU
        FROM dbo.bam_Metadata_DatabaseVersion
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetDatabaseVersion] TO [BAM_ManagementWS]
    AS [dbo];

