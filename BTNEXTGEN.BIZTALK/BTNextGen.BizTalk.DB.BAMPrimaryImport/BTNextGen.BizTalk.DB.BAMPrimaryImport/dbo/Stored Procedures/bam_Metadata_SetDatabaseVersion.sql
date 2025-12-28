CREATE PROCEDURE [dbo].[bam_Metadata_SetDatabaseVersion]
(
    @majorVersion            SMALLINT,
    @minorVersion            SMALLINT,
    @buildVersion            SMALLINT,
    @revisionVersion        SMALLINT,
    @SKU                    SMALLINT
)
AS
    BEGIN TRAN
        TRUNCATE TABLE dbo.bam_Metadata_DatabaseVersion
        
        INSERT dbo.bam_Metadata_DatabaseVersion (MajorVersion, MinorVersion, BuildVersion, RevisionVersion, SKU)
        VALUES (@majorVersion, @minorVersion, @buildVersion, @revisionVersion, @SKU)
    COMMIT TRAN