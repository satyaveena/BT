CREATE TABLE [dbo].[xref_MessageText] (
    [lang]    NVARCHAR (10)   NOT NULL,
    [msgID]   INT             NOT NULL,
    [msgText] NVARCHAR (1000) NOT NULL
);


GO
CREATE CLUSTERED INDEX [CX_xref_MessageText_1]
    ON [dbo].[xref_MessageText]([lang] ASC, [msgID] ASC);

