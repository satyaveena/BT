CREATE TABLE [dbo].[adm_MessageBox] (
    [Id]                       INT              IDENTITY (1, 1) NOT NULL,
    [GroupId]                  INT              NOT NULL,
    [DateModified]             DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [DBServerName]             NVARCHAR (80)    NOT NULL,
    [DBName]                   NVARCHAR (128)   NOT NULL,
    [DisableNewMsgPublication] INT              NOT NULL,
    [ConfigurationState]       INT              NOT NULL,
    [UniqueId]                 UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [IsMasterMsgBox]           INT              NOT NULL,
    [nvcDescription]           NVARCHAR (256)   NULL,
    CONSTRAINT [adm_MessageBox_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_MessageBox_fk_group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[adm_Group] ([Id]),
    CONSTRAINT [adm_MessageBox_unique_key] UNIQUE NONCLUSTERED ([DBServerName] ASC, [DBName] ASC)
);

