CREATE TABLE [dbo].[adm_SendHandler] (
    [Id]                          INT              IDENTITY (1, 1) NOT NULL,
    [GroupId]                     INT              NOT NULL,
    [AdapterId]                   INT              NOT NULL,
    [HostId]                      INT              NULL,
    [IsDefault]                   BIT              DEFAULT ((1)) NOT NULL,
    [CustomCfg]                   NTEXT            NULL,
    [DateModified]                DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SubscriptionId]              UNIQUEIDENTIFIER NULL,
    [uidCustomCfgID]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [uidTransmitLocationSSOAppId] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [nvcDescription]              NVARCHAR (256)   NULL,
    CONSTRAINT [adm_SendHandler_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_SendHandler_fk_adapter] FOREIGN KEY ([AdapterId]) REFERENCES [dbo].[adm_Adapter] ([Id]),
    CONSTRAINT [adm_SendHandler_fk_group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[adm_Group] ([Id]),
    CONSTRAINT [adm_SendHandler_fk_host] FOREIGN KEY ([HostId]) REFERENCES [dbo].[adm_Host] ([Id]),
    CONSTRAINT [adm_SendHandler_unique_key] UNIQUE NONCLUSTERED ([HostId] ASC, [AdapterId] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_SendHandler] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_SendHandler] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_SendHandler] TO [BTS_OPERATORS]
    AS [dbo];

