CREATE TABLE [dbo].[xref_ValueXRef] (
    [valueXRefID]   INT           IDENTITY (1, 1) NOT NULL,
    [valueXRefName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_xref_ValueXRef] PRIMARY KEY CLUSTERED ([valueXRefID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_xref_ValueXRef_1]
    ON [dbo].[xref_ValueXRef]([valueXRefName] ASC);

