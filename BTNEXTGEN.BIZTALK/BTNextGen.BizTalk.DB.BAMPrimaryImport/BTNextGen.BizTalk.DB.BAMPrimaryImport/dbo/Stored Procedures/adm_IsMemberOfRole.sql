CREATE PROCEDURE [dbo].[adm_IsMemberOfRole]
@UserName sysname,
@RoleName sysname,
@IsMember int output
AS
BEGIN

 set @UserName = dbo.adm_FormatNTGroupName(@UserName)

 declare @RoleExist int
 select @RoleExist = count(*) from sysusers where UPPER(name) = UPPER(@RoleName)

 if ( @RoleExist = 0 )
 begin
  set @IsMember = 0
 end
 else
 begin
  create table #RoleMemberInfo
  (
   DbRole sysname,
   MemberName sysname,
   MemberSID varbinary(85)
  )

  insert into #RoleMemberInfo
  exec dbo.sp_helprolemember @RoleName

  select @IsMember = count(*)
  from #RoleMemberInfo
  where UPPER(MemberName) = UPPER(@UserName)

  drop table #RoleMemberInfo
 end
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_IsMemberOfRole] TO [BTS_ADMIN_USERS]
    AS [dbo];

