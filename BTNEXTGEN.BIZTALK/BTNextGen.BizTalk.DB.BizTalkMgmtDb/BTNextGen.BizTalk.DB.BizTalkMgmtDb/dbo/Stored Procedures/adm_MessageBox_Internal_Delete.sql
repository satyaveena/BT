CREATE PROCEDURE [dbo].[adm_MessageBox_Internal_Delete]
@DBServerName nvarchar(80),
@DBName nvarchar(128)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction

  declare @BIZTALK_UNKNOWN_DBID int
  set @BIZTALK_UNKNOWN_DBID = 0

  -- Resolve adm_MessageBox.Id
  declare @MsgBoxId as int
  set @MsgBoxId = @BIZTALK_UNKNOWN_DBID

  declare @IsMasterMsgBox as int
  
  select @MsgBoxId = Id, @IsMasterMsgBox = IsMasterMsgBox
  from adm_MessageBox
  where
   DBServerName = @DBServerName AND
   DBName = @DBName
  
  -- Master MsgBox can not be deleted
  
  if (@IsMasterMsgBox <> 0)
  begin
   set @ErrCode = 0xC0C025BD -- CIS_E_ADMIN_CANT_DELETE_MASTERMSGBOX
   goto exit_proc
  end
  
  -- Delete the adm_MessageBox record
  delete from adm_MessageBox
  where Id = @MsgBoxId

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  
  --TDDS configuration - remove sources for given MsgBox in given pool  
  declare @localized_string_HM_3 as nvarchar(256)
  set @localized_string_HM_3 = N'Health Monitoring'
  declare @localized_string_BI_3 as nvarchar(256)
  set @localized_string_BI_3 = N'Business Monitoring'

  declare @HMSourceName as nvarchar(256)
  select @HMSourceName = @DBServerName + '_' + @DBName
  declare @BISourceName as nvarchar(256)
  select @BISourceName = @DBServerName + '_' + @DBName

  --remove all sources in given pool that correspond to given msg box
   --HM:
  declare @GroupName nvarchar(256)
  set @GroupName = dbo.adm_GetGroupName()

  exec @ErrCode = TDDS_DeleteSource @localized_string_HM_3, @HMSourceName
  if ( @ErrCode <> 0 ) goto exit_proc
   --BI:
  exec @ErrCode = TDDS_DeleteSource @localized_string_BI_3, @BISourceName
  if ( @ErrCode <> 0 ) goto exit_proc

exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback transaction
  return @ErrCode
 end

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_Internal_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

