CREATE TABLE [dbo].[bt_DocumentSpec] (
    [id]                     UNIQUEIDENTIFIER CONSTRAINT [DF_bt_DocumentSpec_id] DEFAULT (newid()) NOT NULL,
    [itemid]                 INT              NOT NULL,
    [assemblyid]             INT              NOT NULL,
    [shareid]                UNIQUEIDENTIFIER NULL,
    [msgtype]                NVARCHAR (2048)  NOT NULL,
    [date_modified]          DATETIME         CONSTRAINT [DF_bt_DocumentSpec_date_modified] DEFAULT (getutcdate()) NOT NULL,
    [body_xpath]             NVARCHAR (2421)  NULL,
    [is_property_schema]     BIT              CONSTRAINT [DF_bt_DocumentSpec_is_property_schema] DEFAULT ((0)) NOT NULL,
    [is_multiroot]           BIT              CONSTRAINT [DF_bt_DocumentSpec_is_multiroot] DEFAULT ((0)) NOT NULL,
    [clr_namespace]          NVARCHAR (256)   NULL,
    [clr_typename]           NVARCHAR (256)   NULL,
    [clr_assemblyname]       NVARCHAR (512)   NULL,
    [schema_root_name]       NVARCHAR (2000)  NULL,
    [xsd_type]               NVARCHAR (30)    NULL,
    [is_tracked]             BIT              CONSTRAINT [DF_bt_DocumentSpec_is_tracked] DEFAULT ((0)) NOT NULL,
    [docspec_name]           AS               (case when [clr_namespace]<>N'' then [clr_namespace]+N'.' else N'' end+[clr_typename]),
    [property_clr_class_fqn] AS               (case [is_property_schema] when (1) then ((case when [clr_namespace]<>N'' then [clr_namespace]+N'.' else N'' end+[property_clr_class])+N',')+[clr_assemblyname] else N'' end),
    [schema_root_clr_fqn]    AS               (case when [clr_namespace]<>N'' then [clr_namespace]+N'.' else N'' end+[clr_typename]),
    [is_flat]                BIT              CONSTRAINT [DF_bt_DocumentSpec_is_flat] DEFAULT ((0)) NOT NULL,
    [property_clr_class]     NVARCHAR (2000)  NULL,
    [description]            NVARCHAR (1024)  NULL,
    CONSTRAINT [fk_bt_documentspec_bt_xmlshare] FOREIGN KEY ([shareid]) REFERENCES [dbo].[bt_XMLShare] ([id]),
    CONSTRAINT [FK_bt_DocumentSpec_bts_assembly] FOREIGN KEY ([assemblyid]) REFERENCES [dbo].[bts_assembly] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [fk_bt_documentspec_bts_item] FOREIGN KEY ([itemid]) REFERENCES [dbo].[bts_item] ([id])
);


GO
CREATE CLUSTERED INDEX [IX_bt_DocumentSpec_clr_namespace]
    ON [dbo].[bt_DocumentSpec]([clr_namespace] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_bt_DocumentSpec_msgtype]
    ON [dbo].[bt_DocumentSpec]([msgtype] ASC, [assemblyid] ASC, [shareid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_bt_DocumentSpec]
    ON [dbo].[bt_DocumentSpec]([id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_bt_DocumentSpec_shareid]
    ON [dbo].[bt_DocumentSpec]([shareid] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_DocumentSpec] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_DocumentSpec] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_DocumentSpec] TO [BTS_OPERATORS]
    AS [dbo];

