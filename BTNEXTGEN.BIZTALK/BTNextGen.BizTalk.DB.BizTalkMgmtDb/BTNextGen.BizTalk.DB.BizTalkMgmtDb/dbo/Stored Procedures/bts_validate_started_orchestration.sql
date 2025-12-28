CREATE PROCEDURE [dbo].[bts_validate_started_orchestration]
@nOrchID int
AS
	--// validate started orchestration, it must have all of its port bindings enlisted
	select sp.nvcName, spg.nvcName from bts_orchestration_port orch_port
	join bts_orchestration_port_binding orch_port_binding on orch_port_binding.nOrcPortID = orch_port.nID
	left outer join bts_sendport sp on sp.nID = orch_port_binding.nSendPortID
	left outer join bts_sendportgroup spg on spg.nID = orch_port_binding.nSpgID
	where orch_port.nOrchestrationID = @nOrchID and orch_port.nPolarity = 1
	and (sp.nPortStatus = 1 or spg.nPortStatus = 1)
	--// validate started orchestration, it must have all the invoked orchestrations started
	select orch2.nvcFullName, assembly.nvcFullName from adm_GetOrchestrationDependencies(@nOrchID, 'D') inv_orch
	join bts_orchestration orch2 on orch2.nID = inv_orch.CalleeSvcId
	join bts_assembly assembly on assembly.nID = orch2.nAssemblyID
	where orch2.nOrchestrationStatus != 3
	--// validate started orchestration, select all the enlisted party which are enlisted under the roles
	--// used by this orchestration and has send ports which are not enlisted or started
	select party.nvcName, role.nvcName, rolelinktype.nvcFullName, assembly.nvcFullName from bts_orchestration orch
	inner join bts_rolelink rolelink on orch.nID = rolelink.nOrchestrationID and rolelink.bImplements = 0
	inner join bts_enlistedparty enlistedparty on enlistedparty.nRoleID = rolelink.nRoleID
	inner join bts_party party on enlistedparty.nPartyID = party.nID
	inner join bts_role role on role.nID = rolelink.nRoleID
	inner join bts_rolelink_type rolelinktype on rolelinktype.nID = role.nRoleLinkTypeID
	inner join bts_assembly assembly on assembly.nID = rolelinktype.nAssemblyID
	where 0 < 	(
				select count(*) from bts_enlistedparty_port_mapping portMapping
				inner join bts_enlistedparty_operation_mapping operationMapping on operationMapping.nPortMappingID = portMapping.nID
				inner join bts_party_sendport partySendPort on operationMapping.nPartySendPortID = partySendPort.nID
				inner join bts_sendport sendport on partySendPort.nSendPortID = sendport.nID
				where sendport.nPortStatus = 1 and enlistedparty.nID = portMapping.nEnlistedPartyID
				)
			and orch.nID = @nOrchID
	group by party.nvcName, role.nvcName, rolelinktype.nvcFullName, assembly.nvcFullName 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_started_orchestration] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_started_orchestration] TO [BTS_OPERATORS]
    AS [dbo];

