CREATE TABLE [dbo].[bts_party_sendport_old] (
    [nID]          INT      IDENTITY (1, 1) NOT NULL,
    [nPartyID]     INT      NOT NULL,
    [nSendPortID]  INT      NOT NULL,
    [nSequence]    INT      NOT NULL,
    [DateModified] DATETIME NOT NULL,
    CONSTRAINT [bts_party_sendport_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_party_sendport_unique_key2] UNIQUE NONCLUSTERED ([nPartyID] ASC, [nSendPortID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_party_sendport_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_party_sendport_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_sendport_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_party_sendport_old] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_sendport_old] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_sendport_old] TO [BTS_OPERATORS]
    AS [dbo];

