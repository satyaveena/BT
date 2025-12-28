CREATE PROCEDURE [dbo].[bam_Metadata_InsertReferencedDatabase]
(
    @serverName            sysname,
    @databaseName        sysname,
    @loginName          NVARCHAR(256)
)
AS
    INSERT dbo.bam_Metadata_ReferencedDatabases (ServerName, DatabaseName, AddedBy, AddedTime)
    VALUES (@serverName, @databaseName, @loginName, GETUTCDATE())