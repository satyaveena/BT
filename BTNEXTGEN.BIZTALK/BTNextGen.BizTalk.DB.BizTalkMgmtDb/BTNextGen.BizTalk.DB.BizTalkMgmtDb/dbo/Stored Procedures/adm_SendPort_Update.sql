CREATE PROCEDURE [dbo].[adm_SendPort_Update]
@Name nvarchar(256),
@PrimaryTransport int,
@AdapterName nvarchar(256),
@InboundTransportURL nvarchar(1024),
@HostName nvarchar(80),
@Comment nvarchar(1024),
@CustomCfg ntext,
@PipelineName nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ReceiveHandlerId as int, @ErrCode as int
 select @ErrCode=0, @ReceiveHandlerId=0

 begin transaction
  
  -- initialize  @ReceiveHandlerId
  if(IsNULL(@HostName, N'') = N'')
  begin
   select @ReceiveHandlerId = NULL
  end
  else
  begin
   select @ReceiveHandlerId = adm_ReceiveHandler.Id from adm_ReceiveHandler, adm_Host, adm_Adapter
   where
    adm_ReceiveHandler.HostId = adm_Host.Id AND
    adm_ReceiveHandler.AdapterId = adm_Adapter.Id AND
    adm_Host.Name = @HostName AND
    adm_Adapter.Name = @AdapterName
    
   if ( 0 = @@ROWCOUNT)
   begin
    set @ErrCode = 0xC0C025C2 -- CIS_E_ADMIN_INVALID_HANDLER_HOST_NAME
    goto exit_proc
   end
  end
/* TODO - implement UPDATE bts_sendport
  update adm_ReceiveLocation
  set
   ReceiveHandlerId = @ReceiveHandlerId,
   Comment = @Comment,
   OperatingWindowEnabled = @OperatingWindowEnabled,
    ActiveStartDT=@ActiveStartDT,
    ActiveStopDT=@ActiveStopDT,
    SrvWinStartDT=@SrvWinStartDT, 
    SrvWinStopDT=@SrvWinStopDT, 
   Disabled = @Disabled,
   CustomCfg = @CustomCfg,
   DateModified = GETUTCDATE(), 
   InboundTransportURL = @InboundTransportURL,
   InboundAddressableURL = @InboundAddressableURL,
   ReceivePipelineId = bts_pipeline.Id
  from bts_pipeline
  where
   bts_pipeline.Name = @ReceiveServiceName AND
   adm_ReceiveLocation.Name = @Name
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
*/   

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
    ON OBJECT::[dbo].[adm_SendPort_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

