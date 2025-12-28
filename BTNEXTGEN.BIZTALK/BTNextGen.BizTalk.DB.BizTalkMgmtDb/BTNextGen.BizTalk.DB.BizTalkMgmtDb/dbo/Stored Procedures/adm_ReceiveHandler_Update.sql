CREATE PROCEDURE [dbo].[adm_ReceiveHandler_Update]
@AdapterName nvarchar(256),
@HostName nvarchar(80),
@CustomCfg ntext,
 --@DefaultHandler int,
@HostNameToSwitchTo nvarchar(80),
@SecureStoreId nvarchar(50),
@ReceiveLocationSSOAppId nvarchar(256),
@Description nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @HandlerId as int, @Capabilities as int
 select @ErrCode = 0, @HandlerId=0, @Capabilities=0

 declare @bHasOpenedTransaction as int
 select @bHasOpenedTransaction=0
 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  select @bHasOpenedTransaction=1
 end
 
  select @HandlerId = adm_ReceiveHandler.Id
  from adm_ReceiveHandler,adm_Host, adm_Adapter
  where adm_ReceiveHandler.HostId = adm_Host.Id AND
   adm_Host.Name = @HostName AND
   adm_Adapter.Name = @AdapterName AND
   adm_ReceiveHandler.AdapterId = adm_Adapter.Id

  -- Ensure that "DefaultHost" property cannot switch off, only swich on.
  --if exists ( select * from adm_DefaultReceiveHandler
  --    where adm_DefaultReceiveHandler.ReceveiHandlerId = @HandlerId AND
  --    0 = @DefaultHandler)  
  --begin
  -- set @ErrCode = 0xC0C02563 -- CIS_E_ADMIN_CORE_DEF_RECEIVE_HANDLER_CANNOT_CHANGE
  -- goto exit_proc
  --end
  
  if(IsNull(@HostNameToSwitchTo, N'') = N'')
   set @HostNameToSwitchTo = @HostName

  -- retreive adapter capabilites
  select @Capabilities = Capabilities from adm_Adapter where Name = @AdapterName
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- Adapter constraint checking
  if ( @HostNameToSwitchTo <> @HostName )
  begin
   declare @HostType as int

   select @HostType = HostType
   from adm_Host
   where Name = @HostNameToSwitchTo
   
   -- adapter requires static handler
   if ( (64 & @Capabilities) <> 0 ) -- eProtocolStaticHandlers
    set @ErrCode = 0xC0C024C2 -- CIS_E_ADMIN_PROTOCOL_STATIC_HANDLERS
   else
   -- check if adapter is creatable, in which case the host must be In-Process
   if ( ((8 & @Capabilities) <> 0) AND (@HostType <> 1) ) -- eHostTypeInProcess
    set @ErrCode = 0xC0C025C7 -- CIS_E_ADMIN_CORE_INPROC_RECEIVE_HANDLER_WRONG_HOST_TYPE
   else
   -- check if adapter is non-creatable, in which case the host must be Isolated
   if ( ((8 & @Capabilities) = 0) AND (@HostType <> 2) ) -- eHostTypeIsolated
    set @ErrCode = 0xC0C025C8 -- CIS_E_ADMIN_CORE_ISOLATED_RECEIVE_HANDLER_WRONG_HOST_TYPE

   if ( @ErrCode <> 0 ) goto exit_proc
  end

  update adm_ReceiveHandler
  set
   HostId = adm_Host.Id,
   CustomCfg = @CustomCfg,
   DateModified = GETUTCDATE(),
   --uidCustomCfgID = convert(uniqueidentifier,@SecureStoreId),
   --uidReceiveLocationSSOAppID = convert(uniqueidentifier,@ReceiveLocationSSOAppId)
   nvcDescription = @Description
  from adm_Host
  where
   adm_ReceiveHandler.Id = @HandlerId AND
   adm_Host.Name = @HostNameToSwitchTo

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- Reassign default handler for the adapter in a group when "DefaultHost=true"
  --update adm_DefaultReceiveHandler
  --set  ReceveiHandlerId = @HandlerId
  --from adm_ReceiveHandler
  --where
  -- 0 <> @DefaultHandler AND
  -- adm_DefaultReceiveHandler.GroupId = @GroupId AND
  -- adm_DefaultReceiveHandler.AdapterId = adm_ReceiveHandler.AdapterId AND
  -- adm_ReceiveHandler.Id = @HandlerId

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
    ON OBJECT::[dbo].[adm_ReceiveHandler_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

