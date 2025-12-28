CREATE PROCEDURE [dbo].[sp_GetRemoteServerName] @ServerName nvarchar(256)
      ,@DatabaseName sysname
      ,@RemoteServerName sysname OUTPUT
AS
 BEGIN
 
 SET NOCOUNT ON

 DECLARE @real_name sysname
 DECLARE @stmt nvarchar(512)

 IF @ServerName IS NULL OR @DatabaseName IS NULL
  RETURN -1

 SELECT @ServerName = replace( @ServerName, '''', '''''' ), @DatabaseName = replace( @DatabaseName, '''', '''''' )
 
 set @stmt = N'[' + @ServerName + N'].[' + @DatabaseName + N'].[dbo].[sp_GetServerName]'
 
 exec @stmt @name=@real_name output
 
 
 IF @real_name IS NULL
  RETURN -1

 SET @RemoteServerName = @real_name

 RETURN 0  
 
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetRemoteServerName] TO [BTS_BACKUP_USERS]
    AS [dbo];

