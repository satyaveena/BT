CREATE TABLE [dbo].[bts_role_porttype] (
    [nID]         INT IDENTITY (1, 1) NOT NULL,
    [nRoleID]     INT NOT NULL,
    [nPortTypeID] INT NOT NULL,
    CONSTRAINT [PK_bts_role_porttype] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_role_porttype_bts_porttype] FOREIGN KEY ([nPortTypeID]) REFERENCES [dbo].[bts_porttype] ([nID]),
    CONSTRAINT [FK_bts_role_porttype_bts_role] FOREIGN KEY ([nRoleID]) REFERENCES [dbo].[bts_role] ([nID]) ON DELETE CASCADE
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_role_porttype] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_role_porttype] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_role_porttype] TO [BTS_OPERATORS]
    AS [dbo];

