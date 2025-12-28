CREATE TABLE [dbo].[xref_MessageArgument] (
    [msgID]          INT           NOT NULL,
    [argSequenceNum] TINYINT       NOT NULL,
    [argName]        NVARCHAR (50) NOT NULL,
    [argIDXRefID]    INT           NULL,
    [argValueXRefID] INT           NULL
);


GO
CREATE CLUSTERED INDEX [CX_xref_MessageArgument]
    ON [dbo].[xref_MessageArgument]([msgID] ASC, [argSequenceNum] ASC);

