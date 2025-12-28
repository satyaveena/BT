CREATE FUNCTION [dbo].[adm_HasPermissionToPerform](@Tasks nvarchar(128))
RETURNS int
AS
BEGIN
	declare @rc as int
	set @rc = 0
	
	if ( @Tasks = N'LoginTasks' )
	begin
		if ( IS_SRVROLEMEMBER ('sysadmin') = 1 OR
			 IS_SRVROLEMEMBER ('securityadmin') = 1 )
		begin
			set @rc = 1
		end
	end
	else
	if ( @Tasks = N'DbAccessTasks' )
	begin
		if ( IS_SRVROLEMEMBER ('sysadmin') = 1 OR
			 IS_MEMBER ('db_owner') = 1 OR
			 IS_MEMBER ('db_accessadmin') = 1 )
		begin
			set @rc = 1
		end
	end
	else
	if ( @Tasks = N'DbRoleTasks' )
	begin
		if ( IS_SRVROLEMEMBER ('sysadmin') = 1 OR
			 IS_MEMBER ('db_owner') = 1 OR
			 IS_MEMBER ('db_securityadmin') = 1 )
		begin
			set @rc = 1
		end
	end
	else
	if ( @Tasks = N'DDLTasks' )
	begin
		if ( IS_SRVROLEMEMBER ('sysadmin') = 1 OR
			 IS_MEMBER ('db_owner') = 1 OR
			 IS_MEMBER ('db_ddladmin') = 1 )
		begin
			set @rc = 1
		end
	end
	
	return @rc
END
