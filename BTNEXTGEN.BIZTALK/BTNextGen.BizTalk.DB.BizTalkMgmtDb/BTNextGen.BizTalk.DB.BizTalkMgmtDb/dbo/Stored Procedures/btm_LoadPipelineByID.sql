CREATE PROCEDURE [dbo].[btm_LoadPipelineByID]
@PipelineID uniqueidentifier,
@Category smallint OUTPUT,
@AssemblyID int OUTPUT,
@Name nvarchar(256) OUTPUT,
@IsStreaming smallint OUTPUT,
@Version int OUTPUT,
@StrongName nvarchar(256) OUTPUT,
@fSuccess int OUTPUT
AS
set nocount on
declare @ID int
SELECT TOP 1 
	@ID = Id,
	@Category = Category,
	@AssemblyID = nAssemblyID,
	@Name = Name,
	@IsStreaming = IsStreaming,
	@Version = Release,
	@StrongName = FullyQualifiedName
	
FROM bts_pipeline WITH (ROWLOCK)
WHERE PipelineID = @PipelineID
IF (@@ROWCOUNT = 1)
BEGIN
	SELECT pc.Sequence, ps.Id, ps.Category, ps.Name, ps.ExecOptions
	FROM  bts_pipeline_stage ps INNER JOIN bts_pipeline_config pc ON pc.StageID=ps.Id
	WHERE pc.PipelineID = @ID
	ORDER BY pc.Sequence
	set @fSuccess = 1
END
ELSE
BEGIN
	set @fSuccess = 0
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btm_LoadPipelineByID] TO [BTS_HOST_USERS]
    AS [dbo];

