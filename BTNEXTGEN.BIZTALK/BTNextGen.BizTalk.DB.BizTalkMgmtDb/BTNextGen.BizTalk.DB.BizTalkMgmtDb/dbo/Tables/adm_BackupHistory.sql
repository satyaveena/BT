CREATE TABLE [dbo].[adm_BackupHistory] (
    [BackupId]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [BackupSetId]        BIGINT          NOT NULL,
    [MarkName]           NVARCHAR (32)   NULL,
    [DatabaseName]       NVARCHAR (128)  NOT NULL,
    [BackupFileName]     NVARCHAR (500)  NOT NULL,
    [BackupFileLocation] NVARCHAR (3000) NOT NULL,
    [BackupType]         CHAR (2)        NOT NULL,
    [BackupDateTime]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [SetComplete]        BIT             DEFAULT ((0)) NOT NULL,
    [ServerName]         NVARCHAR (128)  NULL,
    CONSTRAINT [pk_adm_BackupHistory_BackupId] PRIMARY KEY CLUSTERED ([BackupId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BackupHistory]
    ON [dbo].[adm_BackupHistory]([BackupSetId] ASC, [DatabaseName] ASC, [ServerName] ASC);

