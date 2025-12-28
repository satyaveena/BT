CREATE PROCEDURE [dbo].[sp_BackupBizTalk]
@DBName sysname,
@BackupLocation nvarchar(4000),
@bCompression bit,
@BackupType nvarchar(20)
AS
 declare @Version int
 exec [dbo].[sp_GetServerVersion] @Version output
 if (@Version <=9) -- SQL 2005 or earlier
 BEGIN
  declare @localized_string_compression_option_not_available nvarchar(128)
  set @localized_string_compression_option_not_available = N'Backup compression option is not available on this SQL version. Ignoring compression option'
  if (@bCompression = 1) RAISERROR(@localized_string_compression_option_not_available, 10, 1) WITH LOG

  EXEC ('Backup ' + @BackupType + ' [' + @DBName + '] to DISK=N''' + @BackupLocation + '''')
 END
 else -- SQL 2008 or later
 BEGIN
  if (@bCompression = 0)
   EXEC ('Backup ' + @BackupType + ' [' + @DBName + '] to DISK=N''' + @BackupLocation + ''' WITH NO_COMPRESSION')
  else
   EXEC ('
     BEGIN TRY
      Backup ' + @BackupType + ' [' + @DBName + '] to DISK=N''' + @BackupLocation + ''' WITH COMPRESSION
     END TRY
     BEGIN CATCH
      if (@@ERROR = 3013)
       Backup ' + @BackupType + ' [' + @DBName + '] to DISK=N''' + @BackupLocation + ''' WITH NO_COMPRESSION
     END CATCH
      ')
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_BackupBizTalk] TO [BTS_BACKUP_USERS]
    AS [dbo];

