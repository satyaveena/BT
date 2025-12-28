CREATE PROCEDURE [dbo].[btm_GetPipelineDefinition]
@PipelineFQN nvarchar(256)
AS
set nocount on
SELECT 
	s.Category, s.Name, c.TypeName, sc.Sequence, c.Name, c.CustomData, c.ClsID
FROM 
	bts_pipeline as p, 
	bts_pipeline_config as ps, 
	bts_pipeline_stage as s, 
	bts_component as c, 
	bts_stage_config as sc
WHERE 
	p.FullyQualifiedName = @PipelineFQN AND
	ps.PipelineID = p.Id AND
	ps.StageID = s.Id AND
	sc.StageID = s.Id AND 
	sc.CompID = c.Id
ORDER BY ps.Sequence, sc.Sequence

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btm_GetPipelineDefinition] TO [BTS_ADMIN_USERS]
    AS [dbo];

