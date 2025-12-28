CREATE TABLE [dbo].[bt_Properties] (
    [id]           UNIQUEIDENTIFIER CONSTRAINT [DF_bt_Properties_id] DEFAULT (newid()) NOT NULL,
    [propSchemaID] UNIQUEIDENTIFIER NOT NULL,
    [nAssemblyID]  INT              NULL,
    [msgtype]      NVARCHAR (2048)  NOT NULL,
    [namespace]    NVARCHAR (256)   NOT NULL,
    [name]         NVARCHAR (2048)  NOT NULL,
    [xpath]        NVARCHAR (3357)  NULL,
    [is_tracked]   BIT              CONSTRAINT [DF_bt_Properties_is_tracked] DEFAULT ((0)) NOT NULL,
    [itemid]       INT              NOT NULL,
    CONSTRAINT [PK_bt_Properties] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_bt_Properties_bts_assembly] FOREIGN KEY ([nAssemblyID]) REFERENCES [dbo].[bts_assembly] ([nID]) ON DELETE CASCADE
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_Properties] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bt_Properties] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_Properties] TO [BTS_OPERATORS]
    AS [dbo];

