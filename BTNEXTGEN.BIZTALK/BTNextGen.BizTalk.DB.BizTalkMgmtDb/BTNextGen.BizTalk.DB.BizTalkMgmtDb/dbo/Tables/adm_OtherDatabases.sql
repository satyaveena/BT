CREATE TABLE [dbo].[adm_OtherDatabases] (
    [DefaultDatabaseName] NVARCHAR (128) NOT NULL,
    [DatabaseName]        NVARCHAR (128) NOT NULL,
    [ServerName]          NVARCHAR (80)  NOT NULL,
    [BTSServerName]       NVARCHAR (80)  NOT NULL,
    CONSTRAINT [adm_OtherDatabases_PK] PRIMARY KEY CLUSTERED ([DefaultDatabaseName] ASC, [BTSServerName] ASC)
);

