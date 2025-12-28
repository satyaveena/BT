CREATE PROCEDURE [dbo].[bts_validate_enlisted_parties]
AS
	--// validate all the parties enlisted under roles which are used by the started orchesrations
	select party.nvcName, role.nvcName, rolelinktype.nvcFullName, assembly.nvcFullName from bts_enlistedparty enlistedparty
	inner join bts_party party on enlistedparty.nPartyID = party.nID
	inner join bts_role role on enlistedparty.nRoleID = role.nID
	inner join bts_rolelink_type rolelinktype on rolelinktype.nID = role.nRoleLinkTypeID
	inner join bts_assembly assembly on assembly.nID = rolelinktype.nAssemblyID
	where 0 < 	(
			select count(*) from bts_orchestration orch
			where orch.nOrchestrationStatus >= 3 and 
			role.nID in (select nRoleID from bts_rolelink where nOrchestrationID = orch.nID and bImplements = 0)
			)
	and 0 <		(
			select count(*) from bts_enlistedparty_port_mapping portMapping
			inner join bts_enlistedparty_operation_mapping operationMapping on operationMapping.nPortMappingID = portMapping.nID
			inner join bts_party_sendport partysendport on operationMapping.nPartySendPortID = partysendport.nID
			inner join bts_sendport sendport on partysendport.nSendPortID = sendport.nID and sendport.nPortStatus = 1
			where portMapping.nEnlistedPartyID = enlistedparty.nID
			)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_enlisted_parties] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_enlisted_parties] TO [BTS_OPERATORS]
    AS [dbo];

