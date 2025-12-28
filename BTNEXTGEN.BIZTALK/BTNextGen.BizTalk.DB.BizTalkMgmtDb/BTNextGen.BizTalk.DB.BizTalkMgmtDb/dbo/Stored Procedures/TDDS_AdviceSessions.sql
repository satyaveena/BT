create procedure [dbo].[TDDS_AdviceSessions]
(
	@ServiceID uniqueidentifier
)
as
begin
	declare @@TotalSessionCount int
	declare @@UnassignedSessionsCount int
	declare @@ServiceSessionsCount int
	declare @@ServicesCount int
	declare @@ActiveSessionsCount int
	declare @@AdviceResult int
	declare @@modval int
	declare @@tmpval int
	declare @@SqlCommandTimeout int
	declare @@SourceID uniqueidentifier
	declare @@Destconstr nvarchar(1024)
	declare @@Srcconstr nvarchar(1024)
	declare @@AcceptableLatency int
	declare @@DestID uniqueidentifier
	declare @@StreamType int
	declare @@Intent int
	declare @@ApplockretVal int
	declare @@RefreshInterval int
	declare @@SessionRet int
	declare @@SessionTimeout int
	declare @@DisablesRunningSourcesCount int
		
	
	begin transaction 	
	
	select @@SessionTimeout=SessionTimeout,@@SqlCommandTimeout=SqlCommandTimeout, @@RefreshInterval=RefreshInterval from TDDS_Settings
	set @@SqlCommandTimeout = @@SqlCommandTimeout *1000
	set @@ApplockretVal = 0
	exec @@ApplockretVal = sp_getapplock N'TDDS_AdviceSessions',  N'Exclusive', N'Transaction',@@SqlCommandTimeout
	if (@@ApplockretVal<0)
	begin 
		exec sp_releaseapplock N'TDDS_AdviceSessions', N'Transaction'
		commit transaction 
		if (-1 = @@ApplockretVal)
		begin			
			declare @localized_string_error63100 as nvarchar(128)
			set @localized_string_error63100 = N' TDDS_AdviceSessions Lock request timed out.'
			raiserror(@localized_string_error63100, 16, 1)
			return 63100
		end
		else if (-2 = @@ApplockretVal)
		begin
			declare @localized_string_error63101 as nvarchar(128)
			set @localized_string_error63101 = N' TDDS_AdviceSessions Lock request was cancelled.'
			raiserror(@localized_string_error63101, 16, 1)
			return 63101
		end
		else if (-3 = @@ApplockretVal)
		begin
			declare @localized_string_error63102 as nvarchar(128)
			set @localized_string_error63102 = N' TDDS_AdviceSessions Lock request was chosen as a deadlock victim.'
			raiserror(@localized_string_error63102, 16, 1)
			return 63102
		end
		else
		begin
			declare @localized_string_error63103 as nvarchar(128)
			set @localized_string_error63103 = N' TDDS_AdviceSessions Lock request failed: Unknown Error.'
			raiserror(@localized_string_error63103, 16, 1)
			return 63103
		end
	end
	set @@UnassignedSessionsCount = 0
	set @@ServiceSessionsCount = 0
	set @@ServicesCount = 0
	set @@ActiveSessionsCount = 0
	set @@AdviceResult = 0
	set @@modval = 0
	set @@SessionRet = 0
	set @@DisablesRunningSourcesCount =0
	declare @localized_string_CallAdviceSession as nvarchar(128)
	set @localized_string_CallAdviceSession = N'TDDS_AdviceSessions called'
	
	
	select MAX(TDDS_Heartbeats.TimeLastChanged) as 'TimeLastChanged', TDDS_Heartbeats.ServiceID, TDDS_Heartbeats.SourceID into #PoolUpdates
	from TDDS_Heartbeats with (TABLOCKX)
	where (TDDS_Heartbeats.ServiceID in (select ServiceID from TDDS_Services with (TABLOCK)))
	Group by TDDS_Heartbeats.ServiceID, TDDS_Heartbeats.SourceID
	
	exec TDDS_RecordSessionManagerHeartBeat null, @ServiceID,null,null,null,null,0,@localized_string_CallAdviceSession
	select TDDS_Sources.SourceID, TDDS_Sources.ConnectionString as N'SrcConnstr', 
	       TDDS_Destinations.DestinationID, TDDS_Destinations.ConnectionString as N'DestConnstr',
	       TDDS_Sources.StreamType , TDDS_Sources.AcceptableLatency into #PoolSessions
	from TDDS_Sources with (TABLOCK)
	inner join TDDS_Destinations with (TABLOCK)on 
		( (TDDS_Sources.DestinationID = TDDS_Destinations.DestinationID) And
		  (TDDS_Sources.Enabled =1)
		)
	
	select @@TotalSessionCount=count(*)
	from #PoolSessions
	
	select * into #ActiveSessionsUpdate
	from #PoolUpdates
	where ( datediff(second, TimeLastChanged, GETUTCDATE()) < (@@SessionTimeout) )
	select distinct ServiceID into #ActiveServerList
	from #ActiveSessionsUpdate
	select @@ServicesCount = count(*)
	from #ActiveServerList
	select distinct SourceID into #ActiveSources
	from #ActiveSessionsUpdate
	where (SourceID is not null) and ((SourceID in (select distinct SourceID from #PoolSessions)))
	
	select @@ActiveSessionsCount = count(*)
	from #ActiveSources
	set @@UnassignedSessionsCount = @@TotalSessionCount - @@ActiveSessionsCount
	select  * into #ActiveServiceSessions
	from #ActiveSessionsUpdate
	where (ServiceID = @ServiceID) and (SourceID in (select distinct SourceID from #PoolSessions))
	
	select distinct @@ServiceSessionsCount = count(*)
	from #ActiveServiceSessions
	select distinct #ActiveSessionsUpdate.SourceID, '-1' as 'Intent' into #DisabledRunningSources
	from #ActiveSessionsUpdate
	where ( (#ActiveSessionsUpdate.SourceID not IN (select #PoolSessions.SourceID from  #PoolSessions))
	      And (#ActiveSessionsUpdate.ServiceID = @ServiceID) )
	select distinct @@DisablesRunningSourcesCount = count(*)
	from #DisabledRunningSources
	
	if (not(exists (select ServiceID from #ActiveServerList where ServiceID=@ServiceID)))
	begin
		declare @localized_string_error63104 as nvarchar(128)
		set @localized_string_error63104 = N'Critical error service not registered'
		exec TDDS_RecordSessionManagerHeartBeat null, @ServiceID,null,null,null,null,63104,@localized_string_error63104
		exec sp_releaseapplock N'TDDS_AdviceSessions', N'Transaction'
		commit transaction 
		raiserror(@localized_string_error63104, 16, 1)
		return 63104
	end
	if (@@TotalSessionCount>@@ServicesCount)
	begin
		set @@modval = @@TotalSessionCount % @@ServicesCount
		
		if (0 = @@modval)
		begin
			set @@AdviceResult = (@@TotalSessionCount / @@ServicesCount) - @@ServiceSessionsCount
		end
		else	
		begin
			set @@tmpval = (@@TotalSessionCount - @@modval) / @@ServicesCount
			
			if (@@UnassignedSessionsCount > 0)
			begin
				set @@tmpval = @@tmpval + 1			
			end
			
			set @@AdviceResult = @@tmpval - @@ServiceSessionsCount
		end
	end
	else
	begin
		if (0 = @@TotalSessionCount)
		begin
			exec sp_releaseapplock N'TDDS_AdviceSessions', N'Transaction'
			select * from #DisabledRunningSources
			commit transaction			
			return
		end
		
		if (0 = @@ServiceSessionsCount)
		begin
			set @@AdviceResult = 1
		end
		else if (@@ServiceSessionsCount >= 1)
		begin
			set @@AdviceResult = 1 - @@ServiceSessionsCount
		end
	end
	if (@@AdviceResult > 0)
	begin
		select #PoolSessions.* into #UnassignedSources
		from #PoolSessions
		where (SourceID not in (select distinct SourceID
					from #ActiveSources )) 
					
		if (@@UnassignedSessionsCount =0)
		begin		
			
			declare @localized_string_SessionNotAssigned as nvarchar(128)
			set @localized_string_SessionNotAssigned = N'All Sessions are currently assigned.'
			exec TDDS_RecordSessionManagerHeartBeat null, @ServiceID,null,null,null,null,0,@localized_string_SessionNotAssigned
			commit transaction			
			return
		end
		set ROWCOUNT @@AdviceResult
		
		select *,'1' as 'Intent' into #StartTable
		from #UnassignedSources
		
		set ROWCOUNT  0
		declare ResultCursor CURSOR FOR
		select * from #StartTable
		open ResultCursor
		
		fetch next from ResultCursor into @@SourceID, @@Srcconstr, @@DestID, @@Destconstr, @@StreamType, @@AcceptableLatency,@@Intent
		while @@FETCH_STATUS = 0
		begin
		
			declare @localized_string_Assigned as nvarchar(128)
			set @localized_string_Assigned = N'Assigned'
			exec TDDS_RecordSessionManagerHeartBeat @@SourceID, @ServiceID,null,null,null,null,0,@localized_string_Assigned
			fetch next from ResultCursor into @@SourceID, @@Srcconstr, @@DestID, @@Destconstr, @@StreamType, @@AcceptableLatency, @@Intent
		end
		close ResultCursor
		DEALLOCATE ResultCursor
	end
	else if (@@AdviceResult<0)
	begin
		declare @@tmpval2 int
		set @@tmpval2 =  @@AdviceResult * (-1)
		SET ROWCOUNT  @@tmpval2
		
		select #ActiveServiceSessions.SourceID, '-1' as 'Intent' into #DropTable
		from #ActiveServiceSessions
		SET ROWCOUNT 0
		delete from TDDS_Heartbeats
		where (TDDS_Heartbeats.SourceID in (select distinct #DropTable.SourceID from #DropTable))And
		      (TDDS_Heartbeats.ServiceID = @ServiceID)
					
	end
	declare @localized_string_AdviceRet as nvarchar(512)
	declare @localized_string_SessionsReturned as nvarchar(256)
	declare @string_TDDSAdviceEnd as nvarchar(1024)
	declare @tempString1 as nvarchar(1024)
	declare @tempString2 as nvarchar(1024)
	set @localized_string_AdviceRet = N'TDDS_AdviceSessions Exit, AdviceResult= ' 
	set @localized_string_SessionsReturned  = N', Sessions Returned = '
	if (@@AdviceResult>0)
	begin
		insert into #StartTable
		( #StartTable.SourceID,#StartTable.SrcConnstr,#StartTable.DestinationID,#StartTable.DestConnstr,#StartTable.StreamType,#StartTable.AcceptableLatency, #StartTable.Intent)
		select #DisabledRunningSources.SourceID, '',NEWID ( ),'','',1 , #DisabledRunningSources.Intent from #DisabledRunningSources
		select @@SessionRet=Count(*) from #StartTable
		set @tempString1  = @localized_string_AdviceRet +cast (@@AdviceResult as nvarchar(30))
		set @tempString2  = @localized_string_SessionsReturned +cast (@@SessionRet as nvarchar(30))
		set @string_TDDSAdviceEnd = @tempString1 +  @tempString2
		exec TDDS_RecordSessionManagerHeartBeat null, @ServiceID,null,null,null,null,0, @string_TDDSAdviceEnd
		exec sp_releaseapplock N'TDDS_AdviceSessions', N'Transaction'
		select * from #StartTable
	end
	else if (@@AdviceResult<0)
	begin
		insert into #DropTable
		( #DropTable.SourceID, #DropTable.Intent)
		select #DisabledRunningSources.SourceID, #DisabledRunningSources.Intent from #DisabledRunningSources
		select  @@SessionRet=Count(*) from #DropTable
		set @tempString1  = @localized_string_AdviceRet +cast (@@AdviceResult as nvarchar(30))
		set @tempString2  = @localized_string_SessionsReturned +cast (@@SessionRet as nvarchar(30))
		set @string_TDDSAdviceEnd = @tempString1 +  @tempString2
		exec TDDS_RecordSessionManagerHeartBeat null, @ServiceID,null,null,null,null,0, @string_TDDSAdviceEnd
		exec sp_releaseapplock N'TDDS_AdviceSessions', N'Transaction'
		select  * from #DropTable
	end
	else
	begin
		select  @@SessionRet=Count(*) from #DisabledRunningSources
		set @tempString1  = @localized_string_AdviceRet +cast (@@AdviceResult as nvarchar(30))
		set @tempString2  = @localized_string_SessionsReturned +cast (@@SessionRet as nvarchar(30))
		set @string_TDDSAdviceEnd = @tempString1 +  @tempString2
		exec TDDS_RecordSessionManagerHeartBeat null, @ServiceID,null,null,null,null,0, @string_TDDSAdviceEnd
		exec sp_releaseapplock N'TDDS_AdviceSessions', N'Transaction'
		select * from #DisabledRunningSources
	end
	COMMIT TRANSACTION			
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_AdviceSessions] TO [BAM_CONFIG_READER]
    AS [dbo];

