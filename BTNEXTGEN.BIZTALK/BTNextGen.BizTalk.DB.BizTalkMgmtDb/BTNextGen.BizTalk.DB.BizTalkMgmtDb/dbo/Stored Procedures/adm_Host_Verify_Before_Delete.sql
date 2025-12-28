CREATE PROCEDURE [dbo].[adm_Host_Verify_Before_Delete]
@Name nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 declare @HostId as int
 set @HostId = 0

 -- Resolve foreign key: GroupId, HostId
 select @HostId = Id from adm_Host where Name = @Name

 -- Check that this is not a last host in the group.
 if 1 = (select count(*) from adm_Host)
 begin
  set @ErrCode = 0xC0C0255D -- CIS_E_ADMIN_CANNOT_DELETE_LAST_APP
  goto exit_proc
 end

 -- Check that this is not a "DefaultHost".
 if exists ( select * from adm_Group where DefaultHostId = @HostId)
 begin
  set @ErrCode = 0xC0C0255C -- CIS_E_ADMIN_CANNOT_DELETE_DEFAULT_APP
  goto exit_proc
 end

 -- at least one host in the group must host tracking 
 if not exists (select * from adm_Host
      where  Id <> @HostId AND 0 <> HostTracking)
 begin
  set @ErrCode = 0xC0C0255B -- CIS_E_ADMIN_NO_APP_HOST_TRACKING
  goto exit_proc
 end
 
 -- Check if we have orchestration enlisted to this host
 if exists (select *
    from bts_orchestration
    where nAdminHostID = @HostId)
 begin
  return 0xC0C0251F -- CIS_E_ADMIN_CORE_APP_TYPE_DELETE_HAS_MODULES
  goto exit_proc
 end 

 -- Check if we have ReceiveHandlers associated to this host
 if exists (select *
    from adm_ReceiveHandler
    where HostId = @HostId)
 begin
  return 0xC0C025A9 -- CIS_E_ADMIN_CORE_RH_FOREIGN_KEY
  goto exit_proc
 end 

 -- Check if we have SendHandlers associated to this host
 if exists (select *
    from adm_SendHandler
    where HostId = @HostId)
 begin
  return 0xC0C025AA -- CIS_E_ADMIN_CORE_TH_FOREIGN_KEY
  goto exit_proc
 end 

exit_proc:
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Host_Verify_Before_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

