CREATE TABLE [dbo].[xref_IDXRef] (
    [idXRefID] INT           IDENTITY (1, 1) NOT NULL,
    [idXRef]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_xref_IDXRef] PRIMARY KEY CLUSTERED ([idXRefID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_xref_IDXRef_1]
    ON [dbo].[xref_IDXRef]([idXRef] ASC);

