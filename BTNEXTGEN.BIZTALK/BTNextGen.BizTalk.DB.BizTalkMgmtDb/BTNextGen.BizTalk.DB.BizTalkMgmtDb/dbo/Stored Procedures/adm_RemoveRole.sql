CREATE PROCEDURE [dbo].[adm_RemoveRole]
@RoleName sysname
AS
BEGIN
	declare @RoleExist int
	select @RoleExist = count(*) from sysusers where UPPER(name) = UPPER(@RoleName)
	if ( @RoleExist = 1 )
	begin
		-- If the user don't have sufficient permission, then throw an error and return immediately
		if ( dbo.adm_HasPermissionToPerform('DbRoleTasks') = 0 )
			return 0xC0C025D3 -- CIS_E_ADMIN_CORE_SQL_DBROLE_OPS_INSUFFICIENT_PRIV
		create table #RoleMemberInfo
		(
			DbRole sysname,
			MemberName sysname,
			MemberSID varbinary(85)
		)
		insert into #RoleMemberInfo
		exec dbo.sp_helprolemember @RoleName
		-- Go through each role member and drop it from the role
		declare RoleMemberCursor cursor FOR
			SELECT MemberName
			FROM #RoleMemberInfo 
			
		declare @MemberName sysname
		open RoleMemberCursor
		FETCH NEXT FROM RoleMemberCursor INTO @MemberName
		WHILE (@@FETCH_STATUS = 0)
		BEGIN
			exec dbo.sp_droprolemember @RoleName, @MemberName
			FETCH NEXT FROM RoleMemberCursor INTO @MemberName
		END
		close RoleMemberCursor
		deallocate RoleMemberCursor
		
		drop table #RoleMemberInfo
		
		-- finally, drop the role itself
		exec dbo.sp_droprole @RoleName
	end
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_RemoveRole] TO [BTS_ADMIN_USERS]
    AS [dbo];

