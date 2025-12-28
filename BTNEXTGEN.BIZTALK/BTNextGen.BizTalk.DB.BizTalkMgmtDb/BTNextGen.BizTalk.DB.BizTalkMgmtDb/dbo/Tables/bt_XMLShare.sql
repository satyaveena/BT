CREATE TABLE [dbo].[bt_XMLShare] (
    [id]               UNIQUEIDENTIFIER CONSTRAINT [DF_bt_XMLShare_id] DEFAULT (newid()) NOT NULL,
    [active]           TINYINT          NOT NULL,
    [target_namespace] NVARCHAR (256)   NULL,
    [date_modified]    DATETIME         CONSTRAINT [DF_bt_XMLShare_date_modified] DEFAULT (getutcdate()) NULL,
    [content]          NTEXT            NULL,
    CONSTRAINT [PK_bt_XMLShare] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_bt_XMLShare]
    ON [dbo].[bt_XMLShare]([id] ASC, [active] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_bt_XMLShare_target_namespace]
    ON [dbo].[bt_XMLShare]([target_namespace] ASC, [active] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_XMLShare] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bt_XMLShare] TO [BTS_OPERATORS]
    AS [dbo];

