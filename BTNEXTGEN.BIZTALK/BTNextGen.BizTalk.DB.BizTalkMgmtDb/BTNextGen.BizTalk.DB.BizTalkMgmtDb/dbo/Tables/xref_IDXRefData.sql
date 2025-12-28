CREATE TABLE [dbo].[xref_IDXRefData] (
    [idXRefID]      INT            NOT NULL,
    [appInstanceID] INT            NOT NULL,
    [appID]         NVARCHAR (255) NOT NULL,
    [commonID]      NVARCHAR (50)  NOT NULL,
    CONSTRAINT [IX_xref_IDXRefData_appID] UNIQUE NONCLUSTERED ([appID] ASC, [idXRefID] ASC, [appInstanceID] ASC),
    CONSTRAINT [IX_xref_IDXRefData_commonID] UNIQUE NONCLUSTERED ([commonID] ASC, [idXRefID] ASC, [appInstanceID] ASC)
);


GO
CREATE CLUSTERED INDEX [CIX_xref_IDXRefData]
    ON [dbo].[xref_IDXRefData]([idXRefID] ASC, [appInstanceID] ASC, [appID] ASC, [commonID] ASC);

