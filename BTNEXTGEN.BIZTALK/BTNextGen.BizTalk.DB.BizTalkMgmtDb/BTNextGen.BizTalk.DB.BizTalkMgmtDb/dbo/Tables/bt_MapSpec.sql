CREATE TABLE [dbo].[bt_MapSpec] (
    [id]                  UNIQUEIDENTIFIER CONSTRAINT [DF_bt_MapSpec_id] DEFAULT (newid()) NOT NULL,
    [itemid]              INT              NOT NULL,
    [assemblyid]          INT              NOT NULL,
    [shareid]             UNIQUEIDENTIFIER NULL,
    [indoc_namespace]     NVARCHAR (256)   NULL,
    [outdoc_namespace]    NVARCHAR (256)   NULL,
    [indoc_docspec_name]  NVARCHAR (256)   NULL,
    [outdoc_docspec_name] NVARCHAR (256)   NULL,
    [date_modified]       DATETIME         CONSTRAINT [DF_bt_MapSpec_date_modified] DEFAULT (getutcdate()) NOT NULL,
    [description]         NVARCHAR (1024)  NULL,
    CONSTRAINT [PK_bt_MapSpec] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_bt_mapspec_bt_xmlshare] FOREIGN KEY ([shareid]) REFERENCES [dbo].[bt_XMLShare] ([id]),
    CONSTRAINT [FK_bt_MapSpec_bts_assembly] FOREIGN KEY ([assemblyid]) REFERENCES [dbo].[bts_assembly] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [fk_bt_mapspec_bts_item] FOREIGN KEY ([itemid]) REFERENCES [dbo].[bts_item] ([id])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_MapSpec] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_MapSpec] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_MapSpec] TO [BTS_OPERATORS]
    AS [dbo];

