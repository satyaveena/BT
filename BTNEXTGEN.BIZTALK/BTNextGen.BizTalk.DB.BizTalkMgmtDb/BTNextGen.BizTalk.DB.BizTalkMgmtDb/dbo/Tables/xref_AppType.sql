CREATE TABLE [dbo].[xref_AppType] (
    [appTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [appType]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_xref_AppType] PRIMARY KEY CLUSTERED ([appType] ASC)
);

