CREATE TABLE [dbo].[bts_enlistedparty_port_mapping] (
    [nID]              INT      IDENTITY (1, 1) NOT NULL,
    [nRolePortTypeID]  INT      NOT NULL,
    [nEnlistedPartyID] INT      NOT NULL,
    [DateModified]     DATETIME NOT NULL,
    CONSTRAINT [bts_enlistedparty_port_mapping_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_enlistedparty_port_mapping_foreign_roleporttypeid] FOREIGN KEY ([nRolePortTypeID]) REFERENCES [dbo].[bts_role_porttype] ([nID]),
    CONSTRAINT [bts_enlistedparty_portmapping_foreign_ownerid] FOREIGN KEY ([nEnlistedPartyID]) REFERENCES [dbo].[bts_enlistedparty] ([nID]) ON DELETE CASCADE
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_enlistedparty_port_mapping] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_enlistedparty_port_mapping] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty_port_mapping] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_enlistedparty_port_mapping] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty_port_mapping] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty_port_mapping] TO [BTS_OPERATORS]
    AS [dbo];

