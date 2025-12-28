CREATE PROCEDURE [dbo].[adm_MessageBox_Create]
@DBServerName nvarchar(80),
@DBName nvarchar(128),
@DisableNewMsgPublication int,
@ConfigurationState int,
@Description nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction

  declare @BIZTALK_UNKNOWN_DBID int
  set @BIZTALK_UNKNOWN_DBID = 0

  declare @GroupName as nvarchar(256)

  declare @GroupId as int
  set @GroupId = @BIZTALK_UNKNOWN_DBID

  declare @MsgBoxId as int
  set @MsgBoxId = @BIZTALK_UNKNOWN_DBID

  declare @IsMasterMsgBox int
  set @IsMasterMsgBox = 0
  
  declare @SubscriptionDBServerName nvarchar(80)
  declare @SubscriptionDBName nvarchar(128)
  
  -- Resolve foreign key: GroupId 
  select @GroupId = Id, @GroupName = Name, @SubscriptionDBServerName = SubscriptionDBServerName, @SubscriptionDBName = SubscriptionDBName
  from adm_Group
  
  if ( @GroupId = @BIZTALK_UNKNOWN_DBID )
  begin
   rollback transaction
   return 0xC0C02509 -- CIS_E_ADMIN_CORE_MSGBOX_INVALID_GROUP
  end

  -- If the Message Box is same as the Subscription DB, set this Message Box as the Master Message Box
  
  if ( @SubscriptionDBServerName = @DBServerName AND @SubscriptionDBName = @DBName )
  begin
   set @IsMasterMsgBox = -1
  end
  
  insert into adm_MessageBox
  (
   GroupId, 
   DBServerName, 
   DBName, 
   DisableNewMsgPublication,
   ConfigurationState,
   IsMasterMsgBox,
   nvcDescription
  )
  values
  (
   @GroupId, 
   @DBServerName, 
   @DBName, 
   @DisableNewMsgPublication,
   @ConfigurationState,
   @IsMasterMsgBox,
   @Description
  )
  
  -- check if inserted OK
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  
 -- configure TDDS destinations:
  declare @localized_string_HM_2 as nvarchar(256)
  set @localized_string_HM_2 = N'Health Monitoring'
  declare @localized_string_BI_2 as nvarchar(256)
  set @localized_string_BI_2 = N'Business Monitoring'
 
  declare @HMSourceName as nvarchar(256)
  select @HMSourceName = @DBServerName + '_' + @DBName
  declare @BISourceName as nvarchar(256)
  select @BISourceName = @DBServerName + '_' + @DBName

  
 --HM:
  exec @ErrCode = TDDS_CreateDBSource @localized_string_HM_2, @HMSourceName, @DBServerName, @DBName, 1
  if ( @ErrCode <> 0 ) goto exit_proc
 --BI:
  exec @ErrCode = TDDS_CreateDBSource @localized_string_BI_2, @BISourceName, @DBServerName, @DBName, 0
  if ( @ErrCode <> 0 ) goto exit_proc
 
exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback transaction
 end

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_Create] TO [BTS_ADMIN_USERS]
    AS [dbo];

