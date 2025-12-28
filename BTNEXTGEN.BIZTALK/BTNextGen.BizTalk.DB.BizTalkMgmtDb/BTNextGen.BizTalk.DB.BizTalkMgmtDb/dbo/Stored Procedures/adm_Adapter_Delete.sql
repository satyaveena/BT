CREATE PROCEDURE [dbo].[adm_Adapter_Delete]
@Name nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @AdapterId as int, @Capabilities as int, @dependingRL as int, @dependingTL as int, @bHasOpenedTransaction as int
 select @ErrCode = 0, @AdapterId = 0, @dependingRL = 0, @dependingTL = 0, @bHasOpenedTransaction = 0
 
 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  set @bHasOpenedTransaction=1
 end

  -- retreive adapter Id
  select @AdapterId = Id from adm_Adapter where Name = @Name
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- retreive adapter capabilites
  select @Capabilities = Capabilities from adm_Adapter where Id = @AdapterId
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- check if adapter can be deleted  
  if ( (32 & @Capabilities) <> 0 ) --eProtocolDeleteProtected
   set @ErrCode = 0xC0C024C1 --CIS_E_ADMIN_PROTOCOL_DELETE_PROTECTED
  if ( @ErrCode <> 0 ) goto exit_proc
 
  -- check if there is any depending RLs
  select @dependingRL = count(*)
  from
   adm_ReceiveHandler RH,
   adm_ReceiveLocation RL
  where
   RH.AdapterId = @AdapterId AND
   RH.Id = RL.ReceiveHandlerId
   
  if ( @dependingRL > 0 )
  begin
   set @ErrCode = 0xC0C025A3 -- CIS_E_ADMIN_CORE_DELELE_WITH_EXISTING_RL
   goto exit_proc
  end
  
  -- check if there is any send location of this adapter type
  select @dependingTL = count(*)
  from bts_sendport_transport
  where nTransportTypeId = @AdapterId

  if ( @dependingTL > 0 )
  begin
   set @ErrCode = 0xC0C025A4 -- CIS_E_ADMIN_CORE_DELELE_WITH_EXISTING_TL
   goto exit_proc
  end

  -- we are not currently going to fail users if they have a dynamic sendport enlisted
  -- The subscription will stick around and then will go away when they unenlist the sendport
  -- we will just store enough information so that we can delete the subscription when the sendport is unenlisted
  UPDATE bts_dynamicport_subids set nvcHostName = h.Name
  FROM adm_SendHandler sh 
  JOIN adm_Host h ON sh.HostId = h.Id
  JOIN bts_dynamicport_subids dps ON dps.nSendHandlerID = sh.Id
  WHERE sh.AdapterId = @AdapterId

  delete from adm_ReceiveHandler where AdapterId = @AdapterId

  delete from adm_SendHandler where AdapterId = @AdapterId

  delete from adm_AdapterAlias where AdapterId = @AdapterId

  delete from adm_Adapter where Id = @AdapterId

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
   return @ErrCode
  end
 end

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Adapter_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

