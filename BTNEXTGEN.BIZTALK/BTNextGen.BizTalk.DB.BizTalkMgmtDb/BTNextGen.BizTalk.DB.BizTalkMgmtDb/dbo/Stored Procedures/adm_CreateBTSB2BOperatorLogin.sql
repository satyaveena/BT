CREATE PROCEDURE [dbo].[adm_CreateBTSB2BOperatorLogin]
@LoginName sysname
AS
	declare @rc as int
	exec @rc = adm_AddLoginUser @LoginName, N'BTS_B2B_OPERATORS'
	
	return @rc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_CreateBTSB2BOperatorLogin] TO [BTS_ADMIN_USERS]
    AS [dbo];

