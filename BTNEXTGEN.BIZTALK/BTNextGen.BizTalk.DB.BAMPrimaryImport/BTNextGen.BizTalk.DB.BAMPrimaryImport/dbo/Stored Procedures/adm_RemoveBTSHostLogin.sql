CREATE PROCEDURE [dbo].[adm_RemoveBTSHostLogin]
@LoginName sysname
AS
 -- To handle the case when both BTS Admin NT Group and Host NT Group are the same,
 -- only remove the login from the BTS_HOST_USERS role
 declare @rc as int
 exec @rc = adm_DropDbUserFromRole @LoginName, N'BTS_HOST_USERS'
 
 return @rc
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_RemoveBTSHostLogin] TO [BTS_ADMIN_USERS]
    AS [dbo];

