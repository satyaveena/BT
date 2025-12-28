CREATE TABLE [dbo].[bt_XMLShareReferences] (
    [shareid]          UNIQUEIDENTIFIER NOT NULL,
    [target_namespace] NVARCHAR (256)   NOT NULL,
    CONSTRAINT [FK_bt_XMLShareReferences_bt_XMLShare] FOREIGN KEY ([shareid]) REFERENCES [dbo].[bt_XMLShare] ([id]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [IX_bt_XMLShareReferences]
    ON [dbo].[bt_XMLShareReferences]([shareid] ASC);

