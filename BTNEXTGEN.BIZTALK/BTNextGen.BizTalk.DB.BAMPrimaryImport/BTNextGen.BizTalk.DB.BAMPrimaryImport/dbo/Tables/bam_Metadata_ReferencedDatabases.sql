CREATE TABLE [dbo].[bam_Metadata_ReferencedDatabases] (
    [ServerName]   [sysname]      NOT NULL,
    [DatabaseName] [sysname]      NOT NULL,
    [AddedBy]      NVARCHAR (256) NULL,
    [AddedTime]    DATETIME       NULL
);


GO
CREATE CLUSTERED INDEX [CIndex_ServerAndDatabaseNames]
    ON [dbo].[bam_Metadata_ReferencedDatabases]([ServerName] ASC, [DatabaseName] ASC);

