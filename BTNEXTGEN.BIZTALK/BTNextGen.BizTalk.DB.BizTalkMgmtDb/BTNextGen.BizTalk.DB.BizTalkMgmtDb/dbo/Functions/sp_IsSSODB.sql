CREATE FUNCTION [dbo].[sp_IsSSODB]
(
@BackupServer sysname,
@BackupDB sysname
)
RETURNS bit
as
BEGIN
 declare @ret bit
 set @ret = 0
 if exists(select * from dbo.adm_OtherBackupDatabases where DefaultDatabaseName = N'SSO' and DatabaseName = @BackupDB and ServerName = @BackupServer)
  set @ret = 1
 return @ret
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_IsSSODB] TO [BTS_BACKUP_USERS]
    AS [dbo];

