CREATE PROCEDURE [dbo].[btm_DeletePipeline]
@PipelineID uniqueidentifier,
@fSuccess int OUTPUT
AS
set xact_abort on
DELETE bts_component FROM
   bts_component C INNER JOIN bts_stage_config SC ON SC.CompID=C.Id
   	INNER JOIN bts_pipeline_stage PS ON SC.StageID=PS.Id 
   	INNER JOIN bts_pipeline_config PC ON PS.Id=PC.StageID
   	INNER JOIN bts_pipeline P ON PC.PipelineID=P.Id
   WHERE P.PipelineID=@PipelineID
DELETE bts_stage_config FROM
   bts_stage_config SC INNER JOIN bts_pipeline_stage PS ON SC.StageID=PS.Id 
   	INNER JOIN bts_pipeline_config PC ON PS.Id=PC.StageID
   	INNER JOIN bts_pipeline P ON PC.PipelineID=P.Id
   WHERE P.PipelineID=@PipelineID
DELETE bts_pipeline_stage FROM
   	bts_pipeline_stage PS INNER JOIN bts_pipeline_config PC ON PS.Id=PC.StageID
   	INNER JOIN bts_pipeline P ON PC.PipelineID=P.Id
   WHERE P.PipelineID=@PipelineID
DELETE bts_pipeline_config FROM
   bts_pipeline_config PC INNER JOIN bts_pipeline P ON PC.PipelineID=P.Id
   WHERE P.PipelineID=@PipelineID 
	
DELETE bts_pipeline
	WHERE bts_pipeline.PipelineID=@PipelineID
		
set @fSuccess = 1

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btm_DeletePipeline] TO [BTS_ADMIN_USERS]
    AS [dbo];

