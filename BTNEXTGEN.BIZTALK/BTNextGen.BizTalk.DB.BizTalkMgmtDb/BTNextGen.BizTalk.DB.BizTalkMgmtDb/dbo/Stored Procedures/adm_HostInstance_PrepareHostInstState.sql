CREATE PROCEDURE [dbo].[adm_HostInstance_PrepareHostInstState]
@Name nvarchar(256),
@NewState int,
@LoginName nvarchar(128)
AS
 set nocount on
 set xact_abort on
 
 declare @ErrCode as int
 set @ErrCode = 0
 
 begin transaction

  -- sometimes this store proc is called without logon (e.g. during uninstall)
  if(@LoginName='')
  begin
   select @LoginName=LoginName
   from adm_HostInstance
   where adm_HostInstance.Name = @Name

   set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
   if ( @ErrCode <> 0 ) goto exit_proc
  end
   
  -- Is this LoginName being used by other Host with a DIFFERENT AuthTrusted setting?
  -- If so, then this violates our single trusted domain constraint and we need to
  -- throw an error.

  -- Skip checks for uninstallation
  IF ( @NewState <> 3 AND -- eAPP_INST_CONFIG_UNINSTALL_FAIL
    @NewState <> 5 ) -- eAPP_INST_CONFIG_NOT_INSTALLED
  BEGIN
   DECLARE @AuthTrustedConfig as int
   
   SELECT
    @AuthTrustedConfig = Host.AuthTrusted
   FROM 
    adm_HostInstance as HostInst,
    adm_Server2HostMapping Mapping,
    adm_Host Host
   WHERE
    HostInst.Name = @Name
    AND HostInst.Svr2HostMappingId = Mapping.Id
    AND Mapping.HostId = Host.Id

   IF EXISTS (
    SELECT *
    FROM
     adm_Server2HostMapping Mapping,
     adm_HostInstance HostInst,
     adm_Host Host
    WHERE
     HostInst.Svr2HostMappingId = Mapping.Id
     AND Mapping.HostId = Host.Id
     AND HostInst.ConfigurationState = 1  -- only care about correctly installed HostInstance
     AND HostInst.LoginName = @LoginName
     AND HostInst.Name <> @Name
     AND Host.AuthTrusted <> @AuthTrustedConfig
   )
   BEGIN
    set @ErrCode = 0xC0C025BE -- CIS_E_ADMIN_AUTH_TRUSTED_LOGIN_CONFLICT
    goto exit_proc
   END
  END

  -- Update adm_HostInstance table
  update HostInst
  set ConfigurationState=@NewState,
   DateModified = GETUTCDATE(),
   LoginName = @LoginName
  from  adm_HostInstance as HostInst
  where HostInst.Name = @Name

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
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
    ON OBJECT::[dbo].[adm_HostInstance_PrepareHostInstState] TO [BTS_ADMIN_USERS]
    AS [dbo];

