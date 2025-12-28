CREATE TABLE [dbo].[bts_pipeline] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [PipelineID]         UNIQUEIDENTIFIER NOT NULL,
    [Category]           SMALLINT         NOT NULL,
    [Name]               NVARCHAR (256)   NOT NULL,
    [FullyQualifiedName] NVARCHAR (256)   NOT NULL,
    [IsStreaming]        SMALLINT         CONSTRAINT [DF_bts_pieline_isstreaming] DEFAULT ((0)) NOT NULL,
    [nAssemblyID]        INT              NOT NULL,
    [nvcDescription]     NVARCHAR (1024)  NULL,
    [Release]            INT              NOT NULL,
    CONSTRAINT [bts_pipeline_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([FullyQualifiedName] ASC),
    UNIQUE NONCLUSTERED ([PipelineID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_pipeline] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_pipeline] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_pipeline] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_pipeline] TO [BTS_OPERATORS]
    AS [dbo];

