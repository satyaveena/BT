CREATE TABLE [dbo].[bts_component] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (64)    NOT NULL,
    [Version]      NVARCHAR (10)    NOT NULL,
    [ClsID]        UNIQUEIDENTIFIER NULL,
    [TypeName]     NVARCHAR (256)   NULL,
    [AssemblyPath] NVARCHAR (256)   NULL,
    [Description]  NVARCHAR (256)   NULL,
    [CustomData]   IMAGE            NULL,
    CONSTRAINT [bts_component_pk] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_component] TO [BTS_HOST_USERS]
    AS [dbo];

