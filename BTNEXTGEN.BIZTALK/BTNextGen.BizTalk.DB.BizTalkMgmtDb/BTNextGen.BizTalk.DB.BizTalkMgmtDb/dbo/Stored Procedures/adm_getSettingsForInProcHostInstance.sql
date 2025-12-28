CREATE PROCEDURE [dbo].[adm_getSettingsForInProcHostInstance]
 -- Add the parameters for the stored procedure here
 @HostInstanceUniqueId uniqueIdentifier
AS
 -- SET NOCOUNT ON added to prevent extra result sets from
 -- interfering with SELECT statements.
 set nocount on
 set xact_abort on
 declare @ErrCode as int
 set @ErrCode = 0
 declare @HostId int
 declare @HostInstanceId int
 -- Get HostId from hostinstaceId
 select @HostId = adm_Server2HostMapping.HostId,
        @HostInstanceId = adm_HostInstance.Id
 from
     adm_HostInstance,
     adm_Server2HostMapping
 where (adm_HostInstance.UniqueId = @HostInstanceUniqueId AND
        adm_Server2HostMapping.Id = adm_HostInstance.Svr2HostMappingId)
        
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode
 
 select PropertyName, PropertyValue from adm_GroupSetting
 union
 select 
      'LargeMessageThreshold' as PropertyName,
      CAST(LMSFragmentSize as nvarchar) as PropertyValue from adm_Group
 union
 select 
     'MessageBatchThreshold' as PropertyName,
      CAST(LMSThreshold as nvarchar) as PropertyValue from adm_Group
 union
 select PropertyName , PropertyValue from adm_HostSetting where adm_HostSetting.HostId = @HostId
 union
 select
      'MessagingEngineThreadsPerCpu' as PropertyName,
      CAST(ThreadPoolSize as nvarchar) as PropertyValue from adm_Host where adm_Host.Id = @HostId
 union
 select PropertyName, PropertyValue from adm_HostInstanceSetting where adm_HostInstanceSetting.HostInstanceId = @HostInstanceId
 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_getSettingsForInProcHostInstance] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_getSettingsForInProcHostInstance] TO [BTS_HOST_USERS]
    AS [dbo];

