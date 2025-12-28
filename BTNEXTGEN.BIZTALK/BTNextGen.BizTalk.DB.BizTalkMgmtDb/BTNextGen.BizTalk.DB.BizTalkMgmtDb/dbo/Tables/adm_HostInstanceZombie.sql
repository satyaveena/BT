CREATE TABLE [dbo].[adm_HostInstanceZombie] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (256)   NOT NULL,
    [GroupName]    NVARCHAR (256)   NOT NULL,
    [HostName]     NVARCHAR (80)    NOT NULL,
    [ServerName]   NVARCHAR (63)    NOT NULL,
    [DateModified] DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [NTGroupName]  NVARCHAR (128)   NOT NULL,
    [LoginName]    NVARCHAR (128)   NOT NULL,
    [UniqueId]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [adm_HostInstanceZombie_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_HostInstanceZombie_unique_id] UNIQUE NONCLUSTERED ([UniqueId] ASC)
);

