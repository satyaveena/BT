CREATE PROCEDURE [dbo].[adm_ReceiveHandler_Delete]
@AdapterName nvarchar(256),
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @receiveHandlerId as int, @dependingRL as int
 select @ErrCode = 0, @receiveHandlerId = 0, @dependingRL = 0
 
 declare @bHasOpenedTransaction as int
 select @bHasOpenedTransaction=0
 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  select @bHasOpenedTransaction=1
 end

  -- Check that this is not a "Default Handler".
  --if exists ( select * from adm_DefaultReceiveHandler, adm_ReceiveHandler, adm_Group, adm_Host, adm_Adapter
  --    where adm_DefaultReceiveHandler.ReceveiHandlerId = adm_ReceiveHandler.Id AND
  --    adm_Group.Name = @GroupName AND
  --    adm_Adapter.Name = @AdapterName AND
  --    adm_ReceiveHandler.AdapterId = adm_Adapter.Id AND
  --    adm_Host.Name = @HostName AND
  --    adm_Host.Id = adm_ReceiveHandler.HostId AND
  --    adm_Host.GroupId = adm_Group.Id)
  --begin
  -- set @ErrCode = 0xC0C02561 -- CIS_E_ADMIN_CANNOT_DELETE_DEFAULT_RECEIVE_HANDLER
  -- goto exit_proc
  --end

  select @receiveHandlerId = adm_ReceiveHandler.Id
  from adm_ReceiveHandler,
    adm_Host,
    adm_Adapter
  where adm_Host.Name = @HostName AND
    adm_Adapter.Name = @AdapterName AND
    adm_ReceiveHandler.AdapterId = adm_Adapter.Id AND
    adm_Host.Id = adm_ReceiveHandler.HostId

  -- check if there is any depending RLs
  select @dependingRL = count(*)
  from adm_ReceiveLocation
  where ReceiveHandlerId = @receiveHandlerId
   
  if ( @dependingRL > 0 )
  begin
   set @ErrCode = 0xC0C025A3 -- CIS_E_ADMIN_CORE_DELETE_WITH_EXISTING_RL
   goto exit_proc
  end

  delete adm_ReceiveHandler
  where Id = @receiveHandlerId

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- Make sure that there is at least one receive handler left
  if (not exists (select * from adm_ReceiveHandler, adm_Group, adm_Adapter
       where adm_Adapter.Name = @AdapterName AND
       adm_ReceiveHandler.AdapterId = adm_Adapter.Id))
  begin
   set @ErrCode=0xC0C02569 -- CIS_E_ADMIN_CORE_CANNOT_DELETE_LAST_RECEIVE_HANDLER
   goto exit_proc
  end

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
    ON OBJECT::[dbo].[adm_ReceiveHandler_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

