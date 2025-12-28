CREATE PROCEDURE [dbo].[sp_GetNextBackupSetId]
AS
 BEGIN
 set nocount on

 DECLARE @id   bigint
  ,@local_tran  bit
  ,@ret   int
  ,@error  int
  ,@rowcount int


 IF @@TRANCOUNT > 0
  SET @local_tran = 0
 ELSE
  BEGIN
  BEGIN TRANSACTION
  SET @local_tran = 1
  END

 IF ( SELECT COUNT([NextBackupSetId]) FROM [dbo].[adm_BackupSetId] ) > 1
  GOTO FAILED

 SELECT  @id = [NextBackupSetId]
 FROM [dbo].[adm_BackupSetId] 

 IF @id IS NULL OR @id <=0
  GOTO FAILED

 UPDATE [dbo].[adm_BackupSetId] SET [NextBackupSetId] = @id + 1

 SELECT  @error = @@ERROR
  ,@rowcount = @@ROWCOUNT

 IF @rowcount <> 1 OR @error <> 0
  GOTO FAILED

 IF @local_tran = 1
  COMMIT TRANSACTION

 SET @ret = @id
 GOTO DONE


FAILED:
 IF @local_tran > 0
  ROLLBACK TRANSACTION

 SET @ret = -1
 
 GOTO DONE


DONE: 
 RETURN @ret
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetNextBackupSetId] TO [BTS_BACKUP_USERS]
    AS [dbo];

