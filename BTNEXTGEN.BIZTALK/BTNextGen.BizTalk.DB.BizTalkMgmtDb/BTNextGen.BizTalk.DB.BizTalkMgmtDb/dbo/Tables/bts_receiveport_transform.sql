CREATE TABLE [dbo].[bts_receiveport_transform] (
    [nID]              INT              IDENTITY (1, 1) NOT NULL,
    [nReceivePortID]   INT              NOT NULL,
    [uidTransformGUID] UNIQUEIDENTIFIER NOT NULL,
    [bTransmit]        BIT              NOT NULL,
    [nSequence]        INT              NOT NULL,
    [DateModified]     DATETIME         NOT NULL,
    CONSTRAINT [bts_receiveport_transform_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [CK_applicationbinding_bts_receiveport_transform_schema] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetReceivePortAppId]([nReceivePortID]),[dbo].[adm_GetSchemaAppId]([uidTransformGUID]))=(1)),
    CONSTRAINT [bts_receiveport_transform_foreign_receiveportid] FOREIGN KEY ([nReceivePortID]) REFERENCES [dbo].[bts_receiveport] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [bts_receiveport_transform_foreign_transformid] FOREIGN KEY ([uidTransformGUID]) REFERENCES [dbo].[bt_MapSpec] ([id]),
    CONSTRAINT [bts_receiveport_transform_unique_key2] UNIQUE NONCLUSTERED ([nReceivePortID] ASC, [uidTransformGUID] ASC, [bTransmit] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_receiveport_transform] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_receiveport_transform] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_receiveport_transform] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_receiveport_transform] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_receiveport_transform] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_receiveport_transform] TO [BTS_OPERATORS]
    AS [dbo];

