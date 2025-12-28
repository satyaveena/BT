CREATE PROCEDURE [dbo].[adm_SendHandler_Create]
 --@Name nvarchar(256),
@AdapterName nvarchar(256),
@HostName nvarchar(80),
@CustomCfg ntext,
@SubscriptionId nvarchar(256),
@SecureStoreId nvarchar(256),
@TransmitLocationSSOId nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @AdapterId as int, @HostId as int, @HostType as int, @Capabilities as int
 select @ErrCode = 0, @AdapterId=0, @HostId = 0, @HostType = 0

 declare @bHasOpenedTransaction as int
 select @bHasOpenedTransaction=0
 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  select @bHasOpenedTransaction=1
 end

  select @AdapterId = Id from adm_Adapter where Name = @AdapterName

  select @HostId = Id, @HostType = HostType
  from adm_Host
  where adm_Host.Name = @HostName

  if ( @AdapterId = 0 OR @HostId = 0 )
  begin
   set @ErrCode = 0xC0C0259D -- CIS_E_ADMIN_CORE_TH_INVALID_FOREIGN_KEY_VALUES
   goto exit_proc
  end

  if (N'' = @SubscriptionId)
   set @SubscriptionId = NULL
   
  if exists (select * from adm_SendHandler
       where adm_SendHandler.AdapterId = @AdapterId)
  begin
   set @ErrCode = 0xC0C02564 -- CIS_E_ADMIN_CORE_TRANSMIT_HANDLER_ALREADY_EXISTS
   goto exit_proc
  end

  if ( @HostType <> 1 ) -- eHostTypeInProcess
  begin
   set @ErrCode = 0xC0C025C9 -- CIS_E_ADMIN_CORE_SEND_HANDLER_WRONG_HOST_TYPE
   goto exit_proc
  end

  -- retrieve adapter capabilites
  select @Capabilities = Capabilities from adm_Adapter where Id = @AdapterId
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- check if adapter has static handlers  
  if ( (64 & @Capabilities) <> 0) -- eProtocolStaticHandlers
  begin -- simplify by removing host table
   declare @prvAdapterCnt as int
   select @prvAdapterCnt = count(*) from adm_SendHandler sh
    where sh.AdapterId = @AdapterId 
   if( @prvAdapterCnt > 0 )
    set @ErrCode = 0xC0C024CD -- CIS_E_ADMIN_PROTOCOL_STATIC_SEND_HANDLERS
  end
  if ( @ErrCode <> 0 ) goto exit_proc

  insert into adm_SendHandler
  (
 --  Name,
   AdapterId,
   HostId,
   GroupId,
   CustomCfg,
   SubscriptionId,
   uidCustomCfgID,
   uidTransmitLocationSSOAppId
  )
  values
  (
 --  @Name,
   @AdapterId,
   @HostId,
   dbo.adm_GetGroupId(),
   @CustomCfg,
   @SubscriptionId,
   @SecureStoreId,
   @TransmitLocationSSOId
  )

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  if (dbo.adm_GetNumTransportConflictsInOrg() <> 0)
   set @ErrCode = 0xC0C02829 -- CIS_E_ADMIN_CANNOT_CREATE_SEND_HANDLER_BECAUSE_TRANSPORT_CONSTRAINT

  if ( @ErrCode <> 0 ) goto exit_proc
  
exit_proc:
 if(0 <> @bHasOpenedTransaction)
 begin
  if(@ErrCode = 0)
   commit transaction
  else
  begin
   rollback transaction
   return @ErrCode
  end
 end

 set nocount off
 return @ErrCode

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendHandler_Create] TO [BTS_ADMIN_USERS]
    AS [dbo];

