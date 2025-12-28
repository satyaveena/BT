CREATE TABLE [dbo].[xref_AppInstance] (
    [appInstanceID] INT           IDENTITY (1, 1) NOT NULL,
    [appInstance]   NVARCHAR (50) NOT NULL,
    [appTypeID]     INT           NOT NULL,
    CONSTRAINT [PK_xref_appInstance] PRIMARY KEY CLUSTERED ([appInstanceID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AppInst1]
    ON [dbo].[xref_AppInstance]([appInstance] ASC);

