create procedure [dbo].[sp_BackupAllFull_Schedule] @Frequency nchar(1), @MarkName nvarchar(8), @BackupPath nvarchar(128), @ForceFullBackupAfterPartialSetFailure bit = 0, @BackupHour int=NULL, @UseLocalTime bit = 0
as
 begin 
 set nocount on

 declare @localized_string_sp_BackupAllFull_Schedule_Failed nvarchar(100)
 set @localized_string_sp_BackupAllFull_Schedule_Failed = N'sp_BackupAllFull_Schedule failed'

 declare @localized_string_sp_BackupAllFull_Schedule_Failed_Unknown_Frequency nvarchar(100)
 set @localized_string_sp_BackupAllFull_Schedule_Failed_Unknown_Frequency = N'Unknown value for the parameter @Frequency'

 declare @localized_string_sp_BackupAllFull_Schedule_Failed_Executing_Backup nvarchar(100)
 set @localized_string_sp_BackupAllFull_Schedule_Failed_Executing_Backup = N'Failed running sp_BackupAllFull'

/*
 Start new log shipping strings
*/

 declare @localized_string_sp_BackupAllFull_Schedule_Failed_SelectingForceFull nvarchar(100)
 set @localized_string_sp_BackupAllFull_Schedule_Failed_SelectingForceFull = N'Failed selecting the ForceFull value from the adm_ForceFullBackup table'

 declare @localized_string_sp_BackupAllFull_Schedule_Failed_UpdatingForceFull nvarchar(100)
 set @localized_string_sp_BackupAllFull_Schedule_Failed_UpdatingForceFull = N'Failed updating the ForceFull value in the adm_ForceFullBackup table'

 declare @localized_string_sp_BackupAllFull_Schedule_Failed_SelectHighestPartial nvarchar(100)
 set @localized_string_sp_BackupAllFull_Schedule_Failed_SelectHighestPartial = N'Failed searching for partial backup sets'

/*
 End new log shipping strings
*/

 declare @ret   int
  ,@error   int
  ,@rowcount  int
  ,@errorDesc  nvarchar(128)
  ,@ForceFull  bit
  ,@CurrDT   DateTime
  ,@localtime  datetime

 if (@BackupHour IS NOT NULL) 
 BEGIN
  if (@BackupHour<0) or (@BackupHour>23)
  begin
   select @BackupHour=0
  end
 END
 
 if (@UseLocalTime = 0)
 BEGIN
  set @CurrDT = getutcdate()
  set @localtime = getdate()
 END
 ELSE
 BEGIN
  set @CurrDT = getdate()
  set @localtime = @CurrDT
 END

 /*
  Check to see if we need to backup
 */

 /*
  First check if a forced full backup is requested regardless of any other state
 */
 select top 1 @ForceFull = [ForceFull]
 from [dbo].[adm_BackupSettings]

 select @error  = @@ERROR
  ,@rowcount = @@ROWCOUNT

 if @error <> 0
  begin
  set @errorDesc = @localized_string_sp_BackupAllFull_Schedule_Failed_SelectingForceFull
  goto FAILED
  end

 if @ForceFull = 1
  goto DO_BACKUP
 /*
  Now check if we need to do a full backup based on partial set conditions
 */
 if @ForceFullBackupAfterPartialSetFailure = 1
  begin
  /*
   If a partial set exists with no complete full backup set after it do a full backup
  */
  declare @HighestPartial bigint

  select   @HighestPartial = max(BackupSetId)
  from  [dbo].[adm_BackupHistory]
  where  SetComplete = 0

  if @@ERROR <> 0
   begin
   set @errorDesc = @localized_string_sp_BackupAllFull_Schedule_Failed_SelectHighestPartial
   goto FAILED
   end
  
  if @HighestPartial is not null 
      and not exists( select  1
     from  [dbo].[adm_BackupHistory]
     where  [BackupType] = 'db'
     and  [BackupSetId] > @HighestPartial
     and  [SetComplete] = 1
     group by [BackupSetId] )
   begin
   goto DO_BACKUP
   end
  end

 declare @MaxDT DateTime

 select  @MaxDT = max(BackupDateTime)
 from [dbo].[adm_BackupHistory]
 where BackupType='db'
 /*
  If there's nothing in the table do the backup
 */
 if @MaxDT is null
  goto DO_BACKUP
 /*
  Check to see if we already have a full backup for the specified frequency interval
 */
 
 if @Frequency = 'h' or @Frequency = 'H'
  begin
  if datepart( year, @MaxDT )  = datepart( year, @CurrDT ) and
     datepart( month, @MaxDT ) = datepart( month, @CurrDT ) and
     datepart( day, @MaxDT )   = datepart( day, @CurrDT ) and
     datepart( hour, @MaxDT )  = datepart( hour, @CurrDT )
   goto DONE
  end
 else if @Frequency = 'd' or @Frequency = 'D'
  begin
  if datepart( year, @MaxDT )  = datepart( year, @CurrDT ) and
     datepart( month, @MaxDT ) = datepart( month, @CurrDT ) and
     datepart( day, @MaxDT )   = datepart( day, @CurrDT )
   goto DONE
  end
 else if @Frequency = 'w' or @Frequency = 'W'
  begin
  if datepart( year, @MaxDT )  = datepart( year, @CurrDT ) and
     datepart( month, @MaxDT ) = datepart( month, @CurrDT ) and
     datepart( week, @MaxDT )  = datepart( week, @CurrDT )
   goto DONE
  end
 else if @Frequency = 'm' or @Frequency = 'M'
  begin
  if datepart( year, @MaxDT )  = datepart( year, @CurrDT ) and
     datepart( month, @MaxDT ) = datepart( month, @CurrDT )
   goto DONE
  end
 else if @Frequency = 'y' or @Frequency = 'Y'
  begin
  if datepart( year, @MaxDT )  = datepart( year, @CurrDT )
   goto DONE
  end
 else
  begin
  select @errorDesc = @localized_string_sp_BackupAllFull_Schedule_Failed_Unknown_Frequency
  goto FAILED
  end

 --lets check the hour to backup
 if (@BackupHour is NOT NULL) and
  (@Frequency <> 'h' and @Frequency <> 'H') and  
    (datepart(hour,@localtime) <> @BackupHour)
  goto DONE


DO_BACKUP:
 exec @ret = [dbo].[sp_BackupAllFull] @MarkName, @BackupPath, @CurrDT,@UseLocalTime

 if @@ERROR <> 0 or @ret <> 0 or @ret IS NULL
  begin
  select @errorDesc = @localized_string_sp_BackupAllFull_Schedule_Failed_Executing_Backup
  GOTO FAILED
  end
 else
  begin
  /*
   If this is a forced backup reset the flag so we don't force another
  */
  if @ForceFull = 1
   begin
   update [dbo].[adm_BackupSettings]
   set [ForceFull] = 0

   if @@ERROR <> 0
    begin
    select @errorDesc = @localized_string_sp_BackupAllFull_Schedule_Failed_UpdatingForceFull
    GOTO FAILED
    end
   end

  GOTO DONE
  end

FAILED:
 if @errorDesc is null
  select @errorDesc = @localized_string_sp_BackupAllFull_Schedule_Failed

 raiserror( @errorDesc, 16, -1 )
 return -1

DONE:
 return 0

 end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_BackupAllFull_Schedule] TO [BTS_BACKUP_USERS]
    AS [dbo];

