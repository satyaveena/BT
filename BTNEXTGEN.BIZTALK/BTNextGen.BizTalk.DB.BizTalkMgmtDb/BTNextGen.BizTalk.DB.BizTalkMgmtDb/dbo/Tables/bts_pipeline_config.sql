CREATE TABLE [dbo].[bts_pipeline_config] (
    [PipelineID] INT      NOT NULL,
    [StageID]    INT      NOT NULL,
    [Sequence]   SMALLINT NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_pipeline_config] TO [BTS_HOST_USERS]
    AS [dbo];

