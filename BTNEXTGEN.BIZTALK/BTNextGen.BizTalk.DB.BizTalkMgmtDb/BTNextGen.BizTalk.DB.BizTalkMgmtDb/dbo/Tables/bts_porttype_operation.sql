CREATE TABLE [dbo].[bts_porttype_operation] (
    [nID]         INT            IDENTITY (1, 1) NOT NULL,
    [nPortTypeID] INT            NOT NULL,
    [nvcName]     NVARCHAR (256) NOT NULL,
    [nvcFullName] NVARCHAR (256) NULL,
    [nType]       INT            NOT NULL,
    CONSTRAINT [PK_bts_porttype_operation] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_porttype_operation_bts_porttype] FOREIGN KEY ([nPortTypeID]) REFERENCES [dbo].[bts_porttype] ([nID]) ON DELETE CASCADE
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_porttype_operation] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_porttype_operation] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_porttype_operation] TO [BTS_OPERATORS]
    AS [dbo];

