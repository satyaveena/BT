CREATE PROCEDURE [dbo].[adm_Server_Delete]
@Name nvarchar(63)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @existHostInst as int
 select @ErrCode = 0, @existHostInst = 0

 begin transaction
 
  select @existHostInst = count(*)
  from
   adm_Server svr,
   adm_Server2HostMapping map,
   adm_HostInstance HostInst
  where
   svr.Name = @Name AND
   map.ServerId = svr.Id AND
   map.Id = HostInst.Svr2HostMappingId

  -- Cannot delete server when there are still host instances installed on it
  if ( @existHostInst > 0 )
  begin
   set @ErrCode = 0xC0C0259E -- CIS_E_ADMIN_CORE_SERVER_DELELE_WITH_HostInst
   goto exit_proc
  end

  -- Delete related adm_Server2HostMapping records (for all Hosts in all Groups)
  delete adm_Server2HostMapping
  from adm_Server
  where
   adm_Server.Name=@Name AND
   adm_Server2HostMapping.ServerId = adm_Server.Id

  delete adm_Server
  where Name=@Name

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  
exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback transaction
  return @ErrCode
 end

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

