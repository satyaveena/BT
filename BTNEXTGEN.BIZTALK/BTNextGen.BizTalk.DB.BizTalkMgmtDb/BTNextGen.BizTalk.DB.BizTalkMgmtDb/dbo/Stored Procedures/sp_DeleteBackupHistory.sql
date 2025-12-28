CREATE PROCEDURE [dbo].[sp_DeleteBackupHistory] @DaysToKeep smallint = null, @UseLocalTime bit = 0
AS
 BEGIN
 set nocount on

 IF @DaysToKeep IS NULL OR @DaysToKeep <= 0
  RETURN
 /*
  Only delete full sets
  If a set spans a day such that some items fall into the deleted group and the other don't don't delete the set
  
  Delete history only if history of full Backup exists at a later point of time
  why: history of full backup is used in sp_BackupAllFull_Schedule to check if full backup of databases is required or not. 
  If history of full backup is not present, job will take a full backup irrespective of other options (frequency, Backup hour)
 */
  
 declare @PurgeDateTime datetime
 if (@UseLocalTime = 0)
  set @PurgeDateTime = DATEADD(dd, -@DaysToKeep, GETUTCDATE())
 else
  set @PurgeDateTime = DATEADD(dd, -@DaysToKeep, GETDATE())
 
 DELETE [dbo].[adm_BackupHistory] 
 FROM [dbo].[adm_BackupHistory] [h1]
 WHERE  [BackupDateTime] < @PurgeDateTime
 AND [BackupSetId] NOT IN ( SELECT [BackupSetId] FROM [dbo].[adm_BackupHistory] [h2] WHERE [h2].[BackupSetId] = [h1].[BackupSetId] AND [h2].[BackupDateTime] >= @PurgeDateTime)
 AND EXISTS( SELECT TOP 1 1 FROM [dbo].[adm_BackupHistory] [h2] WHERE [h2].[BackupSetId] > [h1].[BackupSetId] AND [h2].[BackupType] = 'db')
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_DeleteBackupHistory] TO [BTS_BACKUP_USERS]
    AS [dbo];

