CREATE TABLE [dbo].[bts_stage_config] (
    [StageID]  INT      NOT NULL,
    [CompID]   INT      NOT NULL,
    [Sequence] SMALLINT NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_stage_config] TO [BTS_HOST_USERS]
    AS [dbo];

