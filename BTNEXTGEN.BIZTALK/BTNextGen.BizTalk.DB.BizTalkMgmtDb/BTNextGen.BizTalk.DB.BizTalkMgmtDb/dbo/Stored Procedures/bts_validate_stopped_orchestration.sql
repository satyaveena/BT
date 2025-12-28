CREATE PROCEDURE [dbo].[bts_validate_stopped_orchestration]
@nOrchID int
AS
	--// validate unenlisted orchestration, select all started orchestration which invokes this orchestration
	select orch.nvcFullName, assembly.nvcFullName from adm_GetOrchestrationDependencies(@nOrchID, 'U') up
	join bts_orchestration orch on up.CallerSvcId = orch.nID 
	join bts_assembly assembly on assembly.nID = orch.nAssemblyID
	where orch.nOrchestrationStatus >= 3

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_stopped_orchestration] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_stopped_orchestration] TO [BTS_OPERATORS]
    AS [dbo];

