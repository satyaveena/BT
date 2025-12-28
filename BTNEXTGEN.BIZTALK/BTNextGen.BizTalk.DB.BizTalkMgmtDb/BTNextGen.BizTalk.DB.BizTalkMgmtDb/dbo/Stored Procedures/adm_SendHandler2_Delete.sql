CREATE PROCEDURE [dbo].[adm_SendHandler2_Delete]
@AdapterName nvarchar(256),
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @sendHandlerId as int, @dependingSP as int, @IsDefault as bit
 select @ErrCode = 0, @sendHandlerId = 0, @dependingSP = 0
 
 declare @bHasOpenedTransaction as int
 select @bHasOpenedTransaction=0
 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  select @bHasOpenedTransaction=1
 end

 select @IsDefault = adm_SendHandler.IsDefault, @sendHandlerId = adm_SendHandler.Id
 from
  adm_SendHandler,
  adm_Adapter,
  adm_Host
 where
  adm_Adapter.Name = @AdapterName AND
  adm_Host.Name = @HostName AND
  adm_Host.Id = adm_SendHandler.HostId AND
  adm_Adapter.Id = adm_SendHandler.AdapterId
  
 -- disallow if this is a default send handler
 if (@IsDefault = 1)
 begin
  set @ErrCode = 0xC0C0280F -- CIS_E_ADMIN_CANNOT_DELETE_DEFAULT_SEND_HANDLER
  goto exit_proc
 end 

 -- check if there is any depending SPs
 select @dependingSP = count(*)
 from bts_sendport_transport
 where nSendHandlerID = @sendHandlerId
   
 if ( @dependingSP > 0 )
 begin
  set @ErrCode = 0xC0C02811 -- CIS_E_ADMIN_CANNOT_DELETE_SEND_HANDLER_IN_USE
  goto exit_proc
 end

 delete adm_SendHandler
 where Id = @sendHandlerId

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
    ON OBJECT::[dbo].[adm_SendHandler2_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

