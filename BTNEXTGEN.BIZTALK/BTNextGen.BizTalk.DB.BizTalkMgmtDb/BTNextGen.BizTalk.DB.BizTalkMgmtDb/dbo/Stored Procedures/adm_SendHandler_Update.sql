CREATE PROCEDURE [dbo].[adm_SendHandler_Update]
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

 declare @ErrCode as int, @OldHostName as nvarchar(80), @Capabilities as int, @HostType as int
 select @ErrCode = 0, @Capabilities=0, @HostType = 0

 declare @bHasOpenedTransaction as int
 select @bHasOpenedTransaction=0
 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  select @bHasOpenedTransaction=1
 end

  if (N'' = @SubscriptionId)
   set @SubscriptionId = NULL

  -- get name of the old host
  select @OldHostName = adm_Host.Name
  from
   adm_Host,
   adm_Adapter,
   adm_SendHandler
  where
   adm_Adapter.Name = @AdapterName AND
   adm_SendHandler.AdapterId = adm_Adapter.Id AND
   adm_Host.Id = adm_SendHandler.HostId
   
  -- Make sure new host is of type In-Process
  select @HostType = adm_Host.HostType
  from adm_Host
  where Name = @HostName
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  
  if ( @HostType <> 1 ) -- eHostTypeInProcess
  begin
   set @ErrCode = 0xC0C025C9 -- CIS_E_ADMIN_CORE_SEND_HANDLER_WRONG_HOST_TYPE
   goto exit_proc
  end

  -- retreive adapter capabilites
  select @Capabilities = Capabilities from adm_Adapter where Name = @AdapterName
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- check if adapter has static handlers  
  if ( ((64 & @Capabilities) <> 0) AND (@OldHostName <> @HostName) ) -- eProtocolStaticHandlers
  begin
   set @ErrCode = 0xC0C024C2 -- CIS_E_ADMIN_PROTOCOL_STATIC_HANDLERS
   goto exit_proc
  end

  -- Update handler
  update adm_SendHandler
  set
   HostId = adm_Host.Id,
   CustomCfg=@CustomCfg,
   SubscriptionId = IsNULL(@SubscriptionId, adm_SendHandler.SubscriptionId),
   DateModified = GETUTCDATE()
  from
   adm_Host,
   adm_Adapter
  where
   adm_Host.Name = @HostName AND
   adm_Adapter.Name = @AdapterName AND
   adm_SendHandler.AdapterId = adm_Adapter.Id

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

exit_proc:
 if(0 <> @bHasOpenedTransaction)
 begin
  if(@ErrCode = 0)
   commit transaction
  else
  begin
   rollback transaction
  end
 end

 return @ErrCode
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendHandler_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

