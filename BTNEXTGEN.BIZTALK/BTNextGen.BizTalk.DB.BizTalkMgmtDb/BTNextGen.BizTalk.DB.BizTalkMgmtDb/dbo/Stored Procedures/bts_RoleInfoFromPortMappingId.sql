create procedure [dbo].[bts_RoleInfoFromPortMappingId]
@nPortMappingID int
as
set nocount on
select bts_porttype.nvcFullName, bts_role.nvcName, bts_porttype_operation.nvcName
	from
		bts_enlistedparty_port_mapping, 
		bts_role_porttype,
		bts_porttype_operation,
		bts_porttype,
		bts_role
	where
		bts_enlistedparty_port_mapping.nID = @nPortMappingID
		and bts_enlistedparty_port_mapping.nRolePortTypeID = bts_role_porttype.nID
		and bts_porttype_operation.nPortTypeID = bts_role_porttype.nID
		and bts_role_porttype.nPortTypeID = bts_porttype.nID
		and bts_role.nID = bts_role_porttype.nRoleID
set nocount off
