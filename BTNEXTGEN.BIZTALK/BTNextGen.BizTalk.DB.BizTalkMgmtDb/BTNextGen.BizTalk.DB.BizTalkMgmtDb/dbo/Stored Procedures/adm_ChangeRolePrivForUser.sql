	
CREATE PROCEDURE [dbo].[adm_ChangeRolePrivForUser]
@RoleName sysname,
@UserName sysname,
@ProtectType nvarchar(10) -- GRANT/DENY/REVOKE
AS
	set @UserName = dbo.adm_FormatNTGroupName(@UserName)
	if exists ( select * from sysusers where sid = suser_sid(@UserName) )
	begin
		-- If the user don't have sufficient permission, then throw an error and return immediately
		if ( dbo.adm_HasPermissionToPerform('DbRoleTasks') = 0 )
			return 0xC0C025D3 -- CIS_E_ADMIN_CORE_SQL_DBROLE_OPS_INSUFFICIENT_PRIV
		create table #RoleProtectionInfo
		(
			Owner sysname,
			Object sysname,
			Grantee sysname,
			Grantor sysname,
			ProtectType char(10),
			varAction varchar(20),
			snColumn sysname
		)
		insert into #RoleProtectionInfo
		exec dbo.sp_helprotect NULL, @RoleName
		declare RoleInfoCursor cursor FOR
		SELECT Object, varAction FROM #RoleProtectionInfo
		declare @Object sysname
		declare @Action varchar(20)
		open RoleInfoCursor
		FETCH NEXT FROM RoleInfoCursor INTO @Object, @Action
		WHILE (@@FETCH_STATUS = 0)
		BEGIN
			exec(@ProtectType + ' ' + @Action + ' ON ' + @Object + ' TO [' + @UserName + ']')
			FETCH NEXT FROM RoleInfoCursor INTO @Object, @Action
		END
			
		close RoleInfoCursor
		deallocate RoleInfoCursor
		drop table #RoleProtectionInfo
	end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ChangeRolePrivForUser] TO [BTS_ADMIN_USERS]
    AS [dbo];

