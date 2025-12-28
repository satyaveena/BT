CREATE PROCEDURE [dbo].[adm_SendHandler2_Create]
 --@Name nvarchar(256),
@AdapterName nvarchar(256),
@HostName nvarchar(80),
@IsDefault bit,
@CustomCfg ntext,
@SubscriptionId nvarchar(256),
@SecureStoreId nvarchar(256),
@TransmitLocationSSOId nvarchar(256),
@HostNameToSwitchTo nvarchar(80), -- ignored during create
@Description nvarchar(256)
AS
 set nocount on
 set xact_abort on
 declare @ErrCode as int, @AdapterId as int, @HostId as int, @HostType as int, @Capabilities as int, @dps_OldSendHandlerID as int, @dps_NewSendHandlerID as int
 select @ErrCode = 0, @AdapterId=0, @HostId = 0, @HostType = 0, @dps_OldSendHandlerID = 0, @dps_NewSendHandlerID = 0
 declare @bHasOpenedTransaction as int
 select @bHasOpenedTransaction=0
 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  select @bHasOpenedTransaction=1
 end
  -- if this is the first handler of this adapter, set @IsDefault to true
  if (NOT exists (select * from adm_SendHandler, adm_Adapter
         where adm_Adapter.Name = @AdapterName AND
         adm_SendHandler.AdapterId = adm_Adapter.Id ))
   set @IsDefault = 1
  
  
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
   
  if (@IsDefault = 1) AND exists (select * from adm_SendHandler
       where adm_SendHandler.AdapterId = @AdapterId
       and adm_SendHandler.IsDefault = 1)
  begin
   SELECT TOP 1 @dps_OldSendHandlerID = nSendHandlerID FROM bts_dynamicport_subids dps
    INNER JOIN adm_SendHandler sh ON dps.nSendHandlerID = sh.[Id]
   WHERE sh.AdapterId = @AdapterId

   -- if send handler existed and new send handler is default 
   -- then old send handler is not default
   update adm_SendHandler
   set
    IsDefault = 0
   where
    adm_SendHandler.AdapterId = @AdapterId AND
    adm_SendHandler.IsDefault = 1
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
   IsDefault,
   CustomCfg,
   SubscriptionId,
   uidCustomCfgID,
   uidTransmitLocationSSOAppId,
   nvcDescription
  )
  values
  (
 --  @Name,
   @AdapterId,
   @HostId,
   dbo.adm_GetGroupId(),
   @IsDefault,
   @CustomCfg,
   @SubscriptionId,
   @SecureStoreId,
   @TransmitLocationSSOId,
   @Description
  )
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  SELECT @dps_NewSendHandlerID = @@IDENTITY

  if (@IsDefault = 1) AND (@dps_OldSendHandlerID <> 0)
  begin
   -- if send handler existed and new send handler is default 
   -- then old send handler is not default
   -- update host name for dynamic subscriptions to use the new handler

   UPDATE bts_dynamicport_subids 
    SET  nSendHandlerID = @dps_NewSendHandlerID,
      nvcHostName = @HostName
   WHERE nSendHandlerID = @dps_OldSendHandlerID
  end


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
    ON OBJECT::[dbo].[adm_SendHandler2_Create] TO [BTS_ADMIN_USERS]
    AS [dbo];

