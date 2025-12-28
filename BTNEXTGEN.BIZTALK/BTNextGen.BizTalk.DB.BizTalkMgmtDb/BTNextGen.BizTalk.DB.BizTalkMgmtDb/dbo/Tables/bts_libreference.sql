CREATE TABLE [dbo].[bts_libreference] (
    [idapp]   INT            NOT NULL,
    [idlib]   INT            NOT NULL,
    [refName] NVARCHAR (256) NULL,
    CONSTRAINT [fk_bts_libreference_bts_assembly] FOREIGN KEY ([idapp]) REFERENCES [dbo].[bts_assembly] ([nID]),
    CONSTRAINT [fk_bts_libreference_bts_assembly1] FOREIGN KEY ([idlib]) REFERENCES [dbo].[bts_assembly] ([nID])
);

