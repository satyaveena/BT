CREATE TABLE [dbo].[adm_HostInstance] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [Svr2HostMappingId]   INT              NOT NULL,
    [Name]                NVARCHAR (256)   NOT NULL,
    [DateModified]        DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [LoginName]           NVARCHAR (128)   NOT NULL,
    [DisableHostInstance] INT              NOT NULL,
    [ConfigurationState]  INT              NOT NULL,
    [UniqueId]            UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [InstallationContext] NVARCHAR (256)   DEFAULT (N'') NOT NULL,
    [nvcDescription]      NVARCHAR (256)   NULL,
    CONSTRAINT [adm_HostInstance_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_HostInstance_fk_srv2host] FOREIGN KEY ([Svr2HostMappingId]) REFERENCES [dbo].[adm_Server2HostMapping] ([Id]),
    CONSTRAINT [adm_HostInstance_unique_id] UNIQUE NONCLUSTERED ([UniqueId] ASC),
    CONSTRAINT [adm_HostInstance_unique_key] UNIQUE NONCLUSTERED ([Svr2HostMappingId] ASC)
);

