CREATE PROCEDURE [dbo].[bam_Metadata_RemoveReferencedDatabase]
(
    @serverName            sysname,
    @databaseName        sysname
)
AS
    IF NOT EXISTS(SELECT * FROM dbo.bam_Metadata_ReferencedDatabases WHERE ServerName = @serverName AND DatabaseName = @databaseName)
        RAISERROR (N'DbRefDoesNotExist', 16, 1)

    DELETE FROM dbo.bam_Metadata_ReferencedDatabases WHERE ServerName = @serverName AND DatabaseName = @databaseName