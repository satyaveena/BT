CREATE FUNCTION [dbo].[adm_FormatNTGroupName](@login sysname)
RETURNS sysname
AS
BEGIN
 -- If there is no '\' separator, assume this is a local NT group and expand it to '<machine name>\<local NT group>' format
 if ( charindex(N'\', @login) = 0 )
 begin
  set @login = convert(sysname, SERVERPROPERTY('MachineName')) + N'\' + @login
 end
 return @login
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_FormatNTGroupName] TO [BTS_ADMIN_USERS]
    AS [dbo];

