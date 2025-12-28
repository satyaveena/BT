CREATE PROCEDURE [dbo].[bts_validate_unenlisted_sendport]
@nSPID int
AS
	--// validate unenlisted send port, select started send port groups which has no send port enlisted
	select spg.nvcName from bts_sendportgroup spg
	where spg.nPortStatus = 3 and 
		@nSPID IN (select spg_sp.nSendPortID from bts_spg_sendport spg_sp where spg_sp.nSendPortGroupID = spg.nID) and
		(select count(*) from bts_spg_sendport spg_sp 
		join bts_sendport sp on sp.nID = spg_sp.nSendPortID and sp.nPortStatus > 1
		where spg_sp.nSendPortGroupID = spg.nID )= 0
	--// validate unenlisted send port, select started orchestration which has this send port in binding
	select orch.nvcFullName, assembly.nvcFullName from bts_orchestration orch
	inner join bts_assembly assembly on assembly.nID = orch.nAssemblyID
	where orch.nOrchestrationStatus >= 3 and 
		@nSPID IN (	select orch_port_binding.nSendPortID from bts_orchestration_port orch_port 
		join bts_orchestration_port_binding orch_port_binding on orch_port_binding.nOrcPortID = orch_port.nID 
		where orch.nID = orch_port.nOrchestrationID )
		
	--// validate unenlisted send port, select enlisted party which has their roles used by started orchestrations
	select party.nvcName, role.nvcName, rolelinktype.nvcFullName, assembly.nvcFullName from bts_sendport sendport 
	inner join bts_party_sendport partysendport on partysendport.nSendPortID = sendport.nID
	inner join bts_enlistedparty_operation_mapping operationMapping on operationMapping.nPartySendPortID = partysendport.nID
	inner join bts_enlistedparty_port_mapping portMapping on portMapping.nID = operationMapping.nPortMappingID
	inner join bts_enlistedparty enlistedparty on portMapping.nEnlistedPartyID = enlistedparty.nID
	inner join bts_party party on enlistedparty.nPartyID = party.nID
	inner join bts_role role on role.nID = enlistedparty.nRoleID
	inner join bts_rolelink_type rolelinktype on rolelinktype.nID = role.nRoleLinkTypeID
	inner join bts_assembly assembly on assembly.nID = rolelinktype.nAssemblyID
	where sendport.nID = @nSPID and 
		enlistedparty.nRoleID in 
		(
		select distinct(rolelink.nRoleID) from bts_orchestration orch
		inner join bts_rolelink rolelink on rolelink.nOrchestrationID = orch.nID
		where orch.nOrchestrationStatus >= 3 and rolelink.bImplements = 0
		)
	group by party.nvcName, role.nvcName, rolelinktype.nvcFullName, assembly.nvcFullName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_unenlisted_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_unenlisted_sendport] TO [BTS_OPERATORS]
    AS [dbo];

