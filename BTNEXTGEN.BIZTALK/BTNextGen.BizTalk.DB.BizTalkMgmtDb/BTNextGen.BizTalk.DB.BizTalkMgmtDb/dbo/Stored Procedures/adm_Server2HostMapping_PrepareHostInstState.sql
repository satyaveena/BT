CREATE PROCEDURE [dbo].[adm_Server2HostMapping_PrepareHostInstState]
@ServerName nvarchar(63),
@HostName nvarchar(80),
@NewState int
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction

  UPDATE  adm_HostInstance
  set ConfigurationState=@NewState,
   DateModified = GETUTCDATE()
  from
    adm_Server,
   adm_Host,
   adm_Server2HostMapping 
  where
   adm_Server.Name = @ServerName AND
   adm_Host.Name = @HostName AND
   adm_HostInstance.Svr2HostMappingId=adm_Server2HostMapping.Id AND
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_Server2HostMapping.ServerId = adm_Server.Id

  -- olny one isntance should be updated
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback tran
  return @ErrCode
 end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server2HostMapping_PrepareHostInstState] TO [BTS_ADMIN_USERS]
    AS [dbo];

