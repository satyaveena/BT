CREATE PROCEDURE [dbo].[adm_CreateBTSHostLogin]
@LoginName sysname
AS
	declare @rc as int
	exec @rc = adm_AddLoginUser @LoginName, N'BTS_HOST_USERS'
	
	return @rc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_CreateBTSHostLogin] TO [BTS_ADMIN_USERS]
    AS [dbo];

