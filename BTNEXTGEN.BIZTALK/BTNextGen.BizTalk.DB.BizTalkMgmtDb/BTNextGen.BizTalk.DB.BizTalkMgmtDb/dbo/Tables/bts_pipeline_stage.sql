CREATE TABLE [dbo].[bts_pipeline_stage] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Category]    UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (64)    NOT NULL,
    [ExecOptions] INT              NOT NULL,
    CONSTRAINT [bts_pipeline_stage_pk] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_pipeline_stage] TO [BTS_HOST_USERS]
    AS [dbo];

