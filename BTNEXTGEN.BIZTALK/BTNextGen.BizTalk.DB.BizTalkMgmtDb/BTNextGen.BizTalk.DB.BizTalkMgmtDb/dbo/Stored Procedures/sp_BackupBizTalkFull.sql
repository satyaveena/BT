CREATE PROCEDURE [dbo].[sp_BackupBizTalkFull] @seq nvarchar (128), @path nvarchar (3000), @BackupLocation nvarchar(4000) OUTPUT, @bCompression BIT = 0
AS
begin
 declare @DBName nvarchar(128), @ServerName nvarchar(256)

 select @DBName = db_name(), @ServerName = replace( cast( isnull( serverproperty('servername'), '' ) as nvarchar ), '\', '_' ) /* this will be a file path */

 if right( @path, 1 ) = '\'
  SET @BackupLocation=@path + @ServerName + N'_' + @DBName + N'_Full_' + @seq + N'.bak'
 else
  SET @BackupLocation=@path + N'\' + @ServerName + N'_' + @DBName + N'_Full_' + @seq + N'.bak'

 exec sp_BackupBizTalk @DBName, @BackupLocation, @bCompression, N'DATABASE'
 
 if @@ERROR <> 0
  goto FAILED

 return 0

FAILED:
 return -1

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_BackupBizTalkFull] TO [BTS_BACKUP_USERS]
    AS [dbo];

