CREATE PROCEDURE [dbo].[adm_AddDbUserToRole]
@LoginName sysname,
@RoleToGrant sysname
AS
 set @LoginName = dbo.adm_FormatNTGroupName(@LoginName)

 declare @IsMember as int
 exec adm_IsMemberOfRole @LoginName, @RoleToGrant, @IsMember output
 
 if ( @IsMember = 0 )
 begin
  -- If the user don't have sufficient permission, then throw an error and return immediately
  if ( dbo.adm_HasPermissionToPerform('DbRoleTasks') = 0 )
   return 0xC0C025D3 -- CIS_E_ADMIN_CORE_SQL_DBROLE_OPS_INSUFFICIENT_PRIV

  -- If @LoginName is already dbo, skip the sp_addrolemember
  if ( suser_sid(@LoginName) <> (select sid from sysusers where name = 'dbo') )
   exec sp_addrolemember @RoleToGrant, @LoginName
 end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_AddDbUserToRole] TO [BTS_ADMIN_USERS]
    AS [dbo];

