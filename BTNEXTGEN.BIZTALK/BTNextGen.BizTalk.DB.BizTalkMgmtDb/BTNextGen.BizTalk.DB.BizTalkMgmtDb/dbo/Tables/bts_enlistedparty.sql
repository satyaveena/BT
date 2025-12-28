CREATE TABLE [dbo].[bts_enlistedparty] (
    [nID]          INT      IDENTITY (1, 1) NOT NULL,
    [nRoleID]      INT      NOT NULL,
    [nPartyID]     INT      NOT NULL,
    [DateModified] DATETIME NOT NULL,
    CONSTRAINT [bts_enlistedparty_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_enlistedparty_foreign_partyid] FOREIGN KEY ([nPartyID]) REFERENCES [tpm].[Partner] ([PartnerId]),
    CONSTRAINT [bts_enlistedparty_foreign_roleid] FOREIGN KEY ([nRoleID]) REFERENCES [dbo].[bts_role] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [bts_enlistedparty_role_unique_key] UNIQUE NONCLUSTERED ([nRoleID] ASC, [nPartyID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_enlistedparty] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_enlistedparty] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_enlistedparty] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty] TO [BTS_OPERATORS]
    AS [dbo];

