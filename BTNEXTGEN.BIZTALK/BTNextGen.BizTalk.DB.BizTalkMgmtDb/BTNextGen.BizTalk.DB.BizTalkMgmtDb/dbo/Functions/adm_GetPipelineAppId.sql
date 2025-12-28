CREATE FUNCTION [dbo].[adm_GetPipelineAppId] (@pipelineId int) RETURNS int
AS
BEGIN
	declare @nAppId as int
	select @nAppId = nApplicationID 
		from bts_pipeline, bts_assembly 
		where bts_pipeline.Id=@pipelineId 
		and bts_pipeline.nAssemblyID=bts_assembly.nID
	return @nAppId
END
