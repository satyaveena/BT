CREATE PROCEDURE [dbo].[adm_Server2HostMapping_Load]
@ServerName nvarchar(63),
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 -- Resolve Server2MsgBoxMapping.Id
 declare @BIZTALK_UNKNOWN_DBID int
 set @BIZTALK_UNKNOWN_DBID = 0

 declare @Svr2HostMappingId as int
 set @Svr2HostMappingId = @BIZTALK_UNKNOWN_DBID

 select
  @Svr2HostMappingId = adm_Server2HostMapping.Id
 from
  adm_Server2HostMapping,
  adm_Server,
  adm_Host
 where
  adm_Server.Name = @ServerName AND
  adm_Host.Name = @HostName AND
  adm_Server2HostMapping.HostId = adm_Host.Id AND
  adm_Server2HostMapping.ServerId = adm_Server.Id

 -- only one instance should match
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode

 -- Resolve ErrorState
 declare @NumMisconfigured int

 select @NumMisconfigured = count(*)
 from adm_HostInstance
 where
  Svr2HostMappingId = @Svr2HostMappingId AND
  NOT (ConfigurationState = 1 --eAPP_INST_CONFIG_INSTALLED_OK 
  OR ConfigurationState =5 ) -- eAPP_INST_CONFIG_NOT_INSTALLED

 -- declare @ErrorState int
 -- if ( @NumMisconfigured > 0 )
 --  set @ErrorState = 1  -- misconfigured state
 -- else
 --  set @ErrorState = 0  -- okay state

 -- Return the adm_Server2HostMapping record
 select
  adm_Server2HostMapping.Id,
  @ServerName,
  @HostName,
  adm_Server2HostMapping.IsMapped,
  adm_Server2HostMapping.DateModified
 --  @ErrorState
 from
  adm_Server2HostMapping
 where
  adm_Server2HostMapping.Id = @Svr2HostMappingId

 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server2HostMapping_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server2HostMapping_Load] TO [BTS_OPERATORS]
    AS [dbo];

