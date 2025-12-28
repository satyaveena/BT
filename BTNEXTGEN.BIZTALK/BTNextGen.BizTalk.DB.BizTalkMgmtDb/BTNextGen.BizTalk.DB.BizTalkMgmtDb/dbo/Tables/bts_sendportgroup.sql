CREATE TABLE [dbo].[bts_sendportgroup] (
    [nID]            INT              IDENTITY (1, 1) NOT NULL,
    [nvcName]        NVARCHAR (256)   NOT NULL,
    [nPortStatus]    INT              NOT NULL,
    [nvcFilter]      NTEXT            NOT NULL,
    [uidGUID]        UNIQUEIDENTIFIER NOT NULL,
    [nvcCustomData]  NTEXT            NULL,
    [DateModified]   DATETIME         NOT NULL,
    [nApplicationID] INT              NOT NULL,
    [nvcDescription] NVARCHAR (1024)  NULL,
    CONSTRAINT [bts_spg_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [CK_applicationbinding_bts_sendportgroup_sendport] CHECK ([dbo].[adm_ValidateApplicationBinding_Spg]([nApplicationID],[nID])=(1)),
    CONSTRAINT [bts_sendportgroup_foreign_applicationid] FOREIGN KEY ([nApplicationID]) REFERENCES [dbo].[bts_application] ([nID]),
    CONSTRAINT [bts_spg_unique_name] UNIQUE NONCLUSTERED ([nvcName] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_sendportgroup] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_sendportgroup] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendportgroup] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_sendportgroup] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendportgroup] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendportgroup] TO [BTS_OPERATORS]
    AS [dbo];

