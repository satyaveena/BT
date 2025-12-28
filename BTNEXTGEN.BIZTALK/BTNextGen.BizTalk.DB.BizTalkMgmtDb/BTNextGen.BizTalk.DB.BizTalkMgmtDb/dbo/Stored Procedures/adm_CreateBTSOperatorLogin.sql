CREATE PROCEDURE [dbo].[adm_CreateBTSOperatorLogin]
@LoginName sysname
AS
	declare @rc as int
	exec @rc = adm_AddLoginUser @LoginName, N'BTS_OPERATORS'
	
	return @rc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_CreateBTSOperatorLogin] TO [BTS_ADMIN_USERS]
    AS [dbo];

