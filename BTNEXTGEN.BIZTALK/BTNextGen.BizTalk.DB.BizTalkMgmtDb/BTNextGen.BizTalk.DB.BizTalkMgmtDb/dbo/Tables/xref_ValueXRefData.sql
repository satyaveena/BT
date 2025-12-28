CREATE TABLE [dbo].[xref_ValueXRefData] (
    [valueXRefID] INT           NOT NULL,
    [appTypeID]   INT           NOT NULL,
    [appValue]    NVARCHAR (50) NOT NULL,
    [commonValue] NVARCHAR (50) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_xref_ValueXRefData_1]
    ON [dbo].[xref_ValueXRefData]([commonValue] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_xref_ValueXRefData_2]
    ON [dbo].[xref_ValueXRefData]([appValue] ASC);

