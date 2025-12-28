CREATE TABLE [dbo].[bts_sendport_transform] (
    [nID]              INT              IDENTITY (1, 1) NOT NULL,
    [nSendPortID]      INT              NOT NULL,
    [uidTransformGUID] UNIQUEIDENTIFIER NOT NULL,
    [bReceive]         BIT              NOT NULL,
    [nSequence]        INT              NOT NULL,
    [DateModified]     DATETIME         NOT NULL,
    CONSTRAINT [bts_sendport_transform_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [CK_applicationbinding_bts_sendport_transform_schema] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetSendPortAppId]([nSendPortID]),[dbo].[adm_GetSchemaAppId]([uidTransformGUID]))=(1)),
    CONSTRAINT [bts_sendport_transform_foreign_sendportid] FOREIGN KEY ([nSendPortID]) REFERENCES [dbo].[bts_sendport] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [bts_sendport_transform_foreign_transformid] FOREIGN KEY ([uidTransformGUID]) REFERENCES [dbo].[bt_MapSpec] ([id]),
    CONSTRAINT [bts_sendport_transform_unique_key2] UNIQUE NONCLUSTERED ([nSendPortID] ASC, [uidTransformGUID] ASC, [bReceive] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_sendport_transform] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_sendport_transform] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport_transform] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_sendport_transform] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport_transform] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport_transform] TO [BTS_OPERATORS]
    AS [dbo];

