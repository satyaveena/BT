CREATE TABLE [dbo].[bts_party_alias_old] (
    [nID]          INT            IDENTITY (1, 1) NOT NULL,
    [nPartyID]     INT            NOT NULL,
    [nvcName]      NVARCHAR (256) NOT NULL,
    [nvcQualifier] NVARCHAR (64)  NOT NULL,
    [nvcValue]     NVARCHAR (256) NOT NULL,
    [DateModified] DATETIME       NOT NULL,
    CONSTRAINT [bts_party_alias_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_party_alias_unique_qualifiervalue] UNIQUE NONCLUSTERED ([nvcQualifier] ASC, [nvcValue] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_party_alias_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_party_alias_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_alias_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_party_alias_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_alias_old] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_alias_old] TO [BTS_OPERATORS]
    AS [dbo];

