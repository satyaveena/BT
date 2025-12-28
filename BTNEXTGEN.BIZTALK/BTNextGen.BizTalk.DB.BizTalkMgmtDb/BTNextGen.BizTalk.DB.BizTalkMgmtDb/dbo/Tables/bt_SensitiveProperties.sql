CREATE TABLE [dbo].[bt_SensitiveProperties] (
    [id]         INT         IDENTITY (1, 1) NOT NULL,
    [msgtype]    CHAR (2000) NULL,
    [assemblyid] INT         NOT NULL,
    CONSTRAINT [PK_bt_SensitiveProperties] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_bt_SensitiveProperties_bts_assembly] FOREIGN KEY ([assemblyid]) REFERENCES [dbo].[bts_assembly] ([nID]) ON DELETE CASCADE
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_SensitiveProperties] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bt_SensitiveProperties] TO [BTS_ADMIN_USERS]
    AS [dbo];

