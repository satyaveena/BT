CREATE PROCEDURE [dbo].[adm_ReceiveHandler_Create]
 --@Name nvarchar(256),
@AdapterName nvarchar(256),
@HostName nvarchar(80),
@CustomCfg ntext,
 --@DefaultHandler int,
@HostNameToSwitchTo nvarchar(80), -- ignored during create
@SecureStoreId nvarchar(256),
@ReceiveLocationSSOAppId nvarchar(256),
@Description nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @AdapterId as int,  @Capabilities as int, @HostId as int, @HostType as int
 select @ErrCode = 0, @AdapterId=0,  @Capabilities=0, @HostId = 0, @HostType = 0

 declare @bHasOpenedTransaction as int
 select @bHasOpenedTransaction=0
 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  select @bHasOpenedTransaction=1
 end


  -- resolve foreign key values
  select @AdapterId = Id from adm_Adapter where Name = @AdapterName

  select @HostId = Id, @HostType = HostType
  from adm_Host
  where adm_Host.Name = @HostName

  if ( @AdapterId = 0 OR @HostId = 0 )
  begin
   set @ErrCode = 0xC0C0259C -- CIS_E_ADMIN_CORE_RH_INVALID_FOREIGN_KEY_VALUES
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
   select @prvAdapterCnt = count(*) from adm_ReceiveHandler rh
    where rh.AdapterId = @AdapterId 
   if( @prvAdapterCnt > 0 )
    set @ErrCode = 0xC0C024C2 -- CIS_E_ADMIN_PROTOCOL_STATIC_HANDLERS
  end

  -- check if adapter is creatable, in which case the host must be In-Process
  if ( ((8 & @Capabilities) <> 0) AND (@HostType <> 1) ) -- eHostTypeInProcess
   set @ErrCode = 0xC0C025C7 -- CIS_E_ADMIN_CORE_INPROC_RECEIVE_HANDLER_WRONG_HOST_TYPE

  -- check if adapter is non-creatable, in which case the host must be Isolated
  if ( ((8 & @Capabilities) = 0) AND (@HostType <> 2) ) -- eHostTypeIsolated
   set @ErrCode = 0xC0C025C8 -- CIS_E_ADMIN_PROTOCOL_STATIC_HANDLERS

  if ( @ErrCode <> 0 ) goto exit_proc

  insert into adm_ReceiveHandler
  (
   AdapterId,
   HostId,
   GroupId,
   CustomCfg,
   uidCustomCfgID,
   uidReceiveLocationSSOAppID,
   nvcDescription
  )
  values
  (
   @AdapterId,
   @HostId,
   dbo.adm_GetGroupId(),
   @CustomCfg,
   convert(uniqueidentifier,@SecureStoreId),
   convert(uniqueidentifier,@ReceiveLocationSSOAppId),
   @Description
  )

  declare @HandlerId as int
  set @HandlerId = @@Identity
  
  if (dbo.adm_GetNumTransportConflictsInOrg() <> 0)
   set @ErrCode = 0xC0C02580 -- CIS_E_ADMIN_CANNOT_CREATE_RECEIVE_HANDLER_BECAUSE_TRANSPORT_CONSTRAINT

  if ( @ErrCode <> 0 ) goto exit_proc
  
  
  -- If it first handler for this transport in this group, it must be default
  --if not exists (select * from adm_DefaultReceiveHandler where GroupId = @GroupId AND AdapterId = @AdapterId)
  --begin
  -- if (0 <> @DefaultHandler)
  --  begin
  --   insert into adm_DefaultReceiveHandler (GroupId, AdapterId, ReceveiHandlerId)
  --   values (@GroupId, @AdapterId, @HandlerId)
  --  end
  -- else
  --  begin
  --   set @ErrCode = 0xC0C02562 -- CIS_E_ADMIN_FIRST_RECEIVE_HANDLER_MUST_BE_DEFAULT
  --   goto exit_proc
  --  end
  --end

  -- Reassign default handler when "DefaultHandler"="true"
  --if (0 <> @DefaultHandler)
  --begin
  -- update adm_DefaultReceiveHandler
  -- set
  --  ReceveiHandlerId = @HandlerId
  -- where
  --  GroupId = @GroupId AND AdapterId = @AdapterId
  --end
    
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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ReceiveHandler_Create] TO [BTS_ADMIN_USERS]
    AS [dbo];

