CREATE PROCEDURE [dbo].[adm_CreateBTSAdminLogin]
@LoginName sysname
AS
	declare @rc as int
	exec @rc = adm_AddLoginUser @LoginName, N'BTS_ADMIN_USERS'
	
	return @rc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_CreateBTSAdminLogin] TO [BTS_ADMIN_USERS]
    AS [dbo];

