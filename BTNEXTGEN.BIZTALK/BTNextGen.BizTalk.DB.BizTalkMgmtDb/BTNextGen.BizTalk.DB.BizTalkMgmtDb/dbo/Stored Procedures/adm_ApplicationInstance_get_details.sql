CREATE PROCEDURE [dbo].[adm_ApplicationInstance_get_details]
@InstId uniqueidentifier,
@AppInstDateModified datetime OUTPUT,
@AppTypeDateModified datetime OUTPUT,
@ConfigurationCacheRefreshInterval int OUTPUT,
@ConfigurationState int OUTPUT,
@DisableAppInstance int OUTPUT, 
@AppType int OUTPUT, -- TODO: AppType has to be removed
@AdminGroupName nvarchar(256) OUTPUT
AS
	set nocount on
	set xact_abort on
	declare @ErrCode as int, @AppTypeId as int, @MsmqtProtocolId as int, @MsmqtHandlers as int
	select @ErrCode = 0, @AppTypeId = 0, @MsmqtProtocolId = 0, @MsmqtHandlers = 0
	declare @subscriptionDbServer as nvarchar(80), @patternpos as int
	select @subscriptionDbServer = null, @patternpos = 0
	
	select
		@AppInstDateModified = adm_HostInstance.DateModified, 
		@AppTypeDateModified = adm_Host.DateModified,
		@ConfigurationCacheRefreshInterval = adm_Group.ConfigurationCacheRefreshInterval,
		@ConfigurationState = adm_HostInstance.ConfigurationState,
		@DisableAppInstance = adm_HostInstance.DisableHostInstance,
		@AppTypeId = adm_Host.Id,
		@AppType = 1, -- TODO: AppType has to be removed
		@AdminGroupName = BizTalkAdminGroup,
		@subscriptionDbServer = SubscriptionDBServerName
	from
		adm_HostInstance,
		adm_Host,
		adm_Server2HostMapping,
		adm_Group
	where
		adm_HostInstance.UniqueId = @InstId
		AND adm_Server2HostMapping.Id = adm_HostInstance.Svr2HostMappingId
		AND adm_Host.Id = adm_Server2HostMapping.HostId
		AND adm_Group.Id = adm_Host.GroupId
	set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
	if ( @ErrCode <> 0 ) return @ErrCode
	
	select @patternpos =(charindex(N'\',@subscriptionDbServer))
	
	IF (@patternpos <> 0)
	BEGIN --- then it must of the format <machine_name>\<sql server instance>,
	      --- hence strip it down to machine name
	   select @subscriptionDbServer = substring(@subscriptionDbServer,1,@patternpos-1)
	END
	
    select @AdminGroupName = @subscriptionDbServer + N'\' + @AdminGroupName
	
	select 	@MsmqtProtocolId = Id from adm_Adapter where MgmtCLSID = N'{9A7B0162-2CD5-4F61-B7EB-C40A3442A5F8}'
	
	-- Per DCR 36007, MSMQT adapter is now optional
	--	set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
	--	if ( @ErrCode <> 0 ) return @ErrCode
	select @MsmqtHandlers = count(*)
	from adm_SendHandler
	where
		adm_SendHandler.AdapterId = @MsmqtProtocolId AND
		adm_SendHandler.HostId = @AppTypeId
	-- load list of subservices
	select
		adm_HostInstance_SubServices.Name,
		adm_HostInstance_SubServices.MonikerName,
		adm_HostInstance_SubServices.ContextParam,
		adm_HostInstance_SubServices.UniqueId
	from
		adm_HostInstance_SubServices,
		adm_HostInstance,
		adm_Host,
		adm_Server2HostMapping
	where
		adm_HostInstance.UniqueId = @InstId
		AND adm_Server2HostMapping.Id = adm_HostInstance.Svr2HostMappingId
		AND adm_Host.Id = adm_Server2HostMapping.HostId
		AND (adm_HostInstance_SubServices.Type = 0
			 OR ( (adm_HostInstance_SubServices.Type & 1) <> 0 AND adm_Host.HostTracking <> 0)
			 OR ( (adm_HostInstance_SubServices.Type & 16) <> 0 AND @MsmqtHandlers > 0 ))
	order by
		adm_HostInstance_SubServices.Id ASC
	set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ApplicationInstance_get_details] TO [BTS_HOST_USERS]
    AS [dbo];

