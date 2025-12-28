CREATE TABLE [dbo].[adm_ReceiveHandler] (
    [Id]                         INT              IDENTITY (1, 1) NOT NULL,
    [GroupId]                    INT              NOT NULL,
    [AdapterId]                  INT              NOT NULL,
    [HostId]                     INT              NULL,
    [CustomCfg]                  NTEXT            NULL,
    [uidCustomCfgID]             UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [uidReceiveLocationSSOAppID] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [DateModified]               DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [nvcDescription]             NVARCHAR (256)   NULL,
    CONSTRAINT [adm_ReceiveHandler_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_ReceiveHandler_fk_adapter] FOREIGN KEY ([AdapterId]) REFERENCES [dbo].[adm_Adapter] ([Id]),
    CONSTRAINT [adm_ReceiveHandler_fk_group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[adm_Group] ([Id]),
    CONSTRAINT [adm_ReceiveHandler_fk_host] FOREIGN KEY ([HostId]) REFERENCES [dbo].[adm_Host] ([Id]),
    CONSTRAINT [adm_ReceiveHandler_unique_key] UNIQUE NONCLUSTERED ([HostId] ASC, [AdapterId] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_ReceiveHandler] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_ReceiveHandler] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_ReceiveHandler] TO [BTS_OPERATORS]
    AS [dbo];

