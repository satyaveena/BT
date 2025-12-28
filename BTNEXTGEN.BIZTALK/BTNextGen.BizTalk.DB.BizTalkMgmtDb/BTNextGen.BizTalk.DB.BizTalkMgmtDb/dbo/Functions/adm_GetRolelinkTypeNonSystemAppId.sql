CREATE FUNCTION [dbo].[adm_GetRolelinkTypeNonSystemAppId] (@portMappingId int) RETURNS int
AS
BEGIN
	declare @nAppId as int
	select @nAppId = bts_assembly.nApplicationID 
		from
			bts_enlistedparty_port_mapping, 
			bts_role_porttype,
			bts_porttype,
			bts_assembly
		where
				bts_enlistedparty_port_mapping.nID = @portMappingId
			and bts_enlistedparty_port_mapping.nRolePortTypeID = bts_role_porttype.nID
			and bts_role_porttype.nPortTypeID = bts_porttype.nID
			and bts_porttype.nAssemblyID=bts_assembly.nID
		
	-- PF-26539 Return null for system app. This allows to bypass reference enforcement
	declare @nAppIdNoSystemApp as int
 	select @nAppIdNoSystemApp = bts_application.nID
 		from bts_application
 		where isSystem = 0 and nID = @nAppId
	return @nAppIdNoSystemApp
END
