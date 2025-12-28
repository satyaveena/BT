CREATE FUNCTION [dbo].[adm_GetOrchestrationPortAppId] (@orchestrationPortId int) RETURNS int
AS
BEGIN
	declare @nAppId as int
	select @nAppId=bts_assembly.nApplicationID 
		from bts_orchestration_port, bts_orchestration, bts_assembly
		where bts_orchestration_port.nID=@orchestrationPortId
		and bts_orchestration_port.nOrchestrationID=bts_orchestration.nID
		and bts_orchestration.nAssemblyID=bts_assembly.nID
	return @nAppId
END
