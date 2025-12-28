CREATE TABLE [dbo].[bts_spg_sendport] (
    [nID]              INT              IDENTITY (1, 1) NOT NULL,
    [nSendPortGroupID] INT              NOT NULL,
    [nSendPortID]      INT              NOT NULL,
    [uidPrimaryGUID]   UNIQUEIDENTIFIER NOT NULL,
    [uidSecondaryGUID] UNIQUEIDENTIFIER NOT NULL,
    [nSequence]        INT              NOT NULL,
    [DateModified]     DATETIME         NOT NULL,
    CONSTRAINT [bts_spg_sendport_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [CK_applicationbinding_bts_spg_sendport] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetSendPortGroupAppId]([nSendPortGroupID]),[dbo].[adm_GetSendPortAppId]([nSendPortID]))=(1)),
    CONSTRAINT [bts_spg_sendport_foreign_sendportid] FOREIGN KEY ([nSendPortID]) REFERENCES [dbo].[bts_sendport] ([nID]),
    CONSTRAINT [bts_spg_sendport_foreign_spgid] FOREIGN KEY ([nSendPortGroupID]) REFERENCES [dbo].[bts_sendportgroup] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [bts_spg_sendport_unique_key2] UNIQUE NONCLUSTERED ([nSendPortGroupID] ASC, [nSendPortID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_spg_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_spg_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_spg_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_spg_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_spg_sendport] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_spg_sendport] TO [BTS_OPERATORS]
    AS [dbo];

