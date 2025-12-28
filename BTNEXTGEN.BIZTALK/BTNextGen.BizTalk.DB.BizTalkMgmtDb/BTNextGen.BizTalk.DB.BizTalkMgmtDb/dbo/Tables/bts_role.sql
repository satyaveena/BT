CREATE TABLE [dbo].[bts_role] (
    [nID]             INT            IDENTITY (1, 1) NOT NULL,
    [nvcName]         NVARCHAR (256) NOT NULL,
    [nvcFullName]     NVARCHAR (256) NULL,
    [nRoleLinkTypeID] INT            NOT NULL,
    CONSTRAINT [PK_bts_role] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_role_bts_rolelink_type] FOREIGN KEY ([nRoleLinkTypeID]) REFERENCES [dbo].[bts_rolelink_type] ([nID]) ON DELETE CASCADE
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_role] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_role] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_role] TO [BTS_OPERATORS]
    AS [dbo];

