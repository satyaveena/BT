CREATE TABLE [dbo].[xref_MessageDef] (
    [msgID]       INT             IDENTITY (1, 1) NOT NULL,
    [msgCode]     NVARCHAR (50)   NOT NULL,
    [description] NVARCHAR (1000) NULL,
    CONSTRAINT [PK_uan_MessageDef] PRIMARY KEY CLUSTERED ([msgID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_xref_MessageDef_1]
    ON [dbo].[xref_MessageDef]([msgCode] ASC);

