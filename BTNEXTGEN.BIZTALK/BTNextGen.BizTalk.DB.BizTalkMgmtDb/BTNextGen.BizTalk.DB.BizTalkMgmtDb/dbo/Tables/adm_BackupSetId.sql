CREATE TABLE [dbo].[adm_BackupSetId] (
    [NextBackupSetId] BIGINT NULL
);


GO
CREATE TRIGGER adm_BackupSetId_InsertTrigger
ON [dbo].[adm_BackupSetId]
FOR INSERT, UPDATE
AS
 BEGIN 
 set nocount on

 declare @localized_string_adm_BackupSetId_InsertTrigger_Failed_Rowcount nvarchar(128)
 set @localized_string_adm_BackupSetId_InsertTrigger_Failed_Rowcount = N'Rowcount in adm_BackupSetId cannot exceed 1'

 declare @localized_string_adm_BackupSetId_InsertTrigger_Failed_ValueInUse nvarchar(128)
 set @localized_string_adm_BackupSetId_InsertTrigger_Failed_ValueInUse = N'Value cannot exist in adm_BackupHistory.BackupSetId'

 DECLARE @count   int
  ,@updated  bigint
  ,@ErrMsg nvarchar(128)

 SELECT @count = count([NextBackupSetId]) FROM [dbo].[adm_BackupSetId]

 IF @count > 1 
  BEGIN
  SET @ErrMsg = @localized_string_adm_BackupSetId_InsertTrigger_Failed_Rowcount
  GOTO ERROR
  END

 SELECT @updated = NextBackupSetId FROM inserted

 IF EXISTS ( SELECT 1 FROM [dbo].[adm_BackupHistory] WHERE [BackupSetId] = @updated )
  BEGIN
  SET @ErrMsg = @localized_string_adm_BackupSetId_InsertTrigger_Failed_ValueInUse
  GOTO ERROR
  END

 GOTO DONE

ERROR:
 ROLLBACK TRANSACTION
 RAISERROR( @ErrMsg, 16, -1 )

DONE:
 
 END