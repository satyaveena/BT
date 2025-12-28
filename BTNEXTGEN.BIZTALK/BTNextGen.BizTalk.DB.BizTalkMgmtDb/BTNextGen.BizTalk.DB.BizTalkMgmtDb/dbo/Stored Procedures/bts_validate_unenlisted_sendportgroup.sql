CREATE PROCEDURE [dbo].[bts_validate_unenlisted_sendportgroup]
@nSPGID int
AS
	--// validate unenlisted send port group, select all started orchestration which depends upon send port group
	select orch.nvcFullName, assembly.nvcFullName from bts_orchestration orch
	inner join bts_assembly assembly on assembly.nID = orch.nAssemblyID
	where orch.nOrchestrationStatus >= 3 and 
		@nSPGID IN (select orch_port_binding.nSpgID from bts_orchestration_port orch_port 
		join bts_orchestration_port_binding orch_port_binding on orch_port_binding.nOrcPortID = orch_port.nID 
		where orch.nID = orch_port.nOrchestrationID)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_unenlisted_sendportgroup] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_unenlisted_sendportgroup] TO [BTS_OPERATORS]
    AS [dbo];

