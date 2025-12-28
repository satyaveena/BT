CREATE PROCEDURE [dbo].[btm_LoadStageByID]
@StageID int,
@fSuccess int OUTPUT
AS
set nocount on
SELECT s.Sequence, c.ClsID, c.TypeName, c.AssemblyPath, c.CustomData
FROM  bts_component AS c WITH (ROWLOCK),
	bts_stage_config AS s WITH (ROWLOCK)
WHERE s.StageID = @StageID and
	s.CompID = c.Id
ORDER BY s.Sequence
set @fSuccess = 1

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btm_LoadStageByID] TO [BTS_HOST_USERS]
    AS [dbo];

