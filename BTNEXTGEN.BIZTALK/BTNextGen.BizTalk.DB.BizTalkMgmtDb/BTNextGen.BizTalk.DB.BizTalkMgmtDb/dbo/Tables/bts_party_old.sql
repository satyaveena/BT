CREATE TABLE [dbo].[bts_party_old] (
    [nID]                  INT            IDENTITY (1, 1) NOT NULL,
    [nvcName]              NVARCHAR (256) NOT NULL,
    [nvcSignatureCert]     NTEXT          NULL,
    [nvcSignatureCertHash] NVARCHAR (256) NULL,
    [nvcSID]               NVARCHAR (256) NOT NULL,
    [nvcCustomData]        NTEXT          NULL,
    [DateModified]         DATETIME       NOT NULL,
    CONSTRAINT [bts_party_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_party_unique_name] UNIQUE NONCLUSTERED ([nvcName] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_party_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_party_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_party_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_old] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_old] TO [BTS_OPERATORS]
    AS [dbo];

