CREATE PROCEDURE [dbo].[btm_AddStage]
@pipelineID int,
@sequence int,
@Category uniqueidentifier,
@Name nvarchar(64),
@ExecOptions int,
@StageID int OUTPUT
AS
INSERT INTO bts_pipeline_stage (Category, Name, ExecOptions)
		 VALUES (@Category, @Name, @ExecOptions)
SELECT @StageID = @@IDENTITY
INSERT INTO bts_pipeline_config ( PipelineID, StageID, Sequence)
	   VALUES ( @pipelineID, @StageID, @sequence)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btm_AddStage] TO [BTS_ADMIN_USERS]
    AS [dbo];

