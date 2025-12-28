CREATE PROCEDURE [dbo].[adm_SendHandler2_Update]
 --@Name nvarchar(256),
@AdapterName nvarchar(256),
@HostName nvarchar(80),
@IsDefault bit,
@CustomCfg ntext,
@SubscriptionId nvarchar(256),
@SecureStoreId nvarchar(256),
@TransmitLocationSSOId nvarchar(256),
@HostNameToSwitchTo nvarchar(80),
@Description nvarchar(256)
AS
 set nocount on
 set xact_abort on
 declare @ErrCode as int, @Capabilities as int, @HostType as int, @HandlerId as int, @OldDefault as int
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
  
 if (N'' = @HostNameToSwitchTo)
  set @HostNameToSwitchTo = @HostName
 
 -- keep the handler id
 select @HandlerId = adm_SendHandler.Id, @OldDefault = adm_SendHandler.IsDefault
 from adm_SendHandler, adm_Host, adm_Adapter
 where adm_SendHandler.HostId = adm_Host.Id AND
  adm_Host.Name = @HostName AND
  adm_Adapter.Name = @AdapterName AND
  adm_SendHandler.AdapterId = adm_Adapter.Id
  
 -- make sure that IsDefault cannot be turned off
 if ( @OldDefault = 1 AND @IsDefault = 0 )
 begin
  set @ErrCode = 0xC0C02810 -- CIS_E_ADMIN_CANNOT_CHANGE_DEFAULT_SEND_HANDLER_TO_NONDEFAULT
  goto exit_proc
 end
   
 if ( @HostNameToSwitchTo <> @HostName )
 begin  
  -- Make sure new host is of type In-Process
  select @HostType = adm_Host.HostType
  from adm_Host
  where Name = @HostNameToSwitchTo
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  
  if ( @HostType <> 1 ) -- eHostTypeInProcess
  begin
   set @ErrCode = 0xC0C025C9 -- CIS_E_ADMIN_CORE_SEND_HANDLER_WRONG_HOST_TYPE
   goto exit_proc
  end
 end
 -- retreive adapter capabilites
 select @Capabilities = Capabilities from adm_Adapter where Name = @AdapterName
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) goto exit_proc
 -- check if adapter has static handlers  
 if ( ((64 & @Capabilities) <> 0) AND (@HostNameToSwitchTo <> @HostName) ) -- eProtocolStaticHandlers
 begin
  set @ErrCode = 0xC0C024C2 -- CIS_E_ADMIN_PROTOCOL_STATIC_HANDLERS
  goto exit_proc
 end
 -- If we are setting this send handler to default, then update the old default handler
 if ( @OldDefault = 0 AND @IsDefault = 1 )
 begin
  
  -- won't allow changing default send handler if there are enlisted dynamic send ports
  -- if ( select count(*) from bts_sendport where bDynamic = 1 AND nPortStatus = 2 ) > 0
  -- begin
  -- set @ErrCode = 0xC0C02812  -- CIS_E_ADMIN_ENLISTED_DYNAMIC_SEND_PORT_EXIST
  -- goto exit_proc
  -- end
  
  -- get the old default handler id  
  declare @OldDefaultHandlerId as int
  select @OldDefaultHandlerId = adm_SendHandler.Id
  from adm_SendHandler,  adm_Adapter
  where 
   adm_Adapter.Name = @AdapterName AND
   adm_SendHandler.AdapterId = adm_Adapter.Id AND
   adm_SendHandler.IsDefault = 1
  
  -- update send handler ID and host name for dynamic subscriptions to use the new handler
  update bts_dynamicport_subids
  set
   nSendHandlerID = @HandlerId,
   nvcHostName = @HostNameToSwitchTo
  where
   nSendHandlerID = @OldDefaultHandlerId
   
  -- update send handler table to turn off the default flag on old default handler
  update adm_SendHandler
  set
   IsDefault = 0
  where
   Id = @OldDefaultHandlerId
  
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  
  
  
 end   
   
 -- Update this handler
 update adm_SendHandler
 set
  HostId = adm_Host.Id,
  IsDefault = @IsDefault,
  CustomCfg=@CustomCfg,
  SubscriptionId = IsNULL(@SubscriptionId, adm_SendHandler.SubscriptionId),
  DateModified = GETUTCDATE(),
  nvcDescription = @Description
 from
  adm_Host
 where
  adm_SendHandler.Id = @HandlerId AND
  adm_Host.Name = @HostNameToSwitchTo
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
    ON OBJECT::[dbo].[adm_SendHandler2_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

