CREATE TABLE [dbo].[adm_Server2HostMapping] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [ServerId]     INT      NOT NULL,
    [HostId]       INT      NOT NULL,
    [DateModified] DATETIME DEFAULT (getutcdate()) NOT NULL,
    [IsMapped]     INT      NOT NULL,
    CONSTRAINT [adm_Server2HostMapping_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_Server2HostMapping_fk_host] FOREIGN KEY ([HostId]) REFERENCES [dbo].[adm_Host] ([Id]),
    CONSTRAINT [adm_Server2HostMapping_fk_server] FOREIGN KEY ([ServerId]) REFERENCES [dbo].[adm_Server] ([Id]),
    CONSTRAINT [adm_Server2HostMapping_unique_key] UNIQUE NONCLUSTERED ([ServerId] ASC, [HostId] ASC)
);

