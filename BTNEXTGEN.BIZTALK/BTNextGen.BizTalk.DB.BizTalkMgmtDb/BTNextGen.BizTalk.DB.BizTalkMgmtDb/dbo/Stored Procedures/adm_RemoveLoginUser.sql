CREATE PROCEDURE [dbo].[adm_RemoveLoginUser]
@LoginName sysname
AS
	-- If the user don't have sufficient permission, then throw an error and return immediately
	if ( dbo.adm_HasPermissionToPerform('DbAccessTasks') = 0 )
		return 0xC0C025D2 -- CIS_E_ADMIN_CORE_SQL_DBACCESS_OPS_INSUFFICIENT_PRIV
	set @LoginName = dbo.adm_FormatNTGroupName(@LoginName)
	-- Remove database user access	
	if exists (select * from sysusers where sid = suser_sid(@LoginName) and hasdbaccess = 1)
		exec sp_revokedbaccess @LoginName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_RemoveLoginUser] TO [BTS_ADMIN_USERS]
    AS [dbo];

