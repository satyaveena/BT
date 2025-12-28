CREATE PROCEDURE [dbo].[adm_Host_PrepareUpdateDelete]
@Name nvarchar(80),
@AuthTrusted int,
@HostTracking int
AS
 set nocount on
 set xact_abort on
 
 declare @ErrCode as int
 set @ErrCode = 0

 -- Check if AuthTrusted and HostTracking setting have changed or not
 declare @ExistAuthTrusted as int, @ExistHostTracking as int, @AuthTrustedChanged as int, @HostTrackingChanged as int
 select @ExistAuthTrusted = 0, @ExistHostTracking = 0, @AuthTrustedChanged = 0, @HostTrackingChanged = 0
 
 declare @ExistHostNTGroup as nvarchar(128)
 select @ExistHostNTGroup = N''
 
 declare @HostId as int
 
 select
  @ExistAuthTrusted = AuthTrusted,
  @ExistHostTracking = HostTracking,
  @ExistHostNTGroup = NTGroupName,
  @HostId = Id
 from
  adm_Host
 where
  Name = @Name
 
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode

 if ( @ExistAuthTrusted <> @AuthTrusted )
  set @AuthTrustedChanged = -1

 if ( @ExistHostTracking <> @HostTracking )
  set @HostTrackingChanged = -1

 -- Check if this is Host.NTGroupName is still being used by any other Host(s) for host tracking
 declare @LastNTGroupToHostTracking as int
 set @LastNTGroupToHostTracking = 1
  
 if exists (
  select * from adm_Host
  where
   Name <> @Name
   AND NTGroupName = @ExistHostNTGroup
   AND HostTracking <> 0
 )
 begin
  set @LastNTGroupToHostTracking = 0
 end

 declare @NumInstalledHostInst as int
 select @NumInstalledHostInst = count(*)
 from adm_Host host, adm_Server2HostMapping mapping, adm_HostInstance inst
 where
  @HostId = mapping.HostId
  AND mapping.Id = inst.Svr2HostMappingId
  AND inst.ConfigurationState = 1 -- eAPP_INST_CONFIG_INSTALLED_OK
  
 -- Return the results via recordset
 select @AuthTrustedChanged, @HostTrackingChanged, @LastNTGroupToHostTracking, @NumInstalledHostInst

 return @ErrCode
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Host_PrepareUpdateDelete] TO [BTS_ADMIN_USERS]
    AS [dbo];

