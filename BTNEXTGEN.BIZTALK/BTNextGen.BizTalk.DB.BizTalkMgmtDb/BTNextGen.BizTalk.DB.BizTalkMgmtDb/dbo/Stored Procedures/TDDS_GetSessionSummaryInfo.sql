create procedure [dbo].[TDDS_GetSessionSummaryInfo]
as
begin
	set nocount on
	declare @@SessionTimeout int
	select @@SessionTimeout=SessionTimeout from TDDS_Settings
	select   TDDS_Services.ServiceID,TDDS_Sources.SourceName, TDDS_Sources.SourceID, TDDS_Services.ServerName, TDDS_Destinations.DestinationName, 
		MAX(TDDS_Heartbeats.TimeLastChanged) as 'LastProcessTime',
		MAX(TDDS_Heartbeats.Latency) as 'MaxL',
		MIN(TDDS_Heartbeats.TimeLastChanged) as 'MinTime',
		Max(TDDS_Heartbeats.Age) as 'MaxAge',
		SUM(TDDS_Heartbeats.RecordsProcessed) as 'Recs'
		into #tmp	
	from TDDS_Services
	inner join TDDS_Heartbeats on TDDS_Services.ServiceID = TDDS_Heartbeats.ServiceID
	inner join TDDS_Sources on  TDDS_Heartbeats.SourceID = TDDS_Sources.SourceID
	inner join TDDS_Destinations on TDDS_Destinations.DestinationID = TDDS_Sources.DestinationID
	Group by TDDS_Sources.SourceName, TDDS_Sources.SourceID, TDDS_Services.ServerName, TDDS_Services.ServiceID, TDDS_Destinations.DestinationName	
	HAVING (datediff(second, MAX(TDDS_Heartbeats.TimeLastChanged), GETUTCDATE()) < (@@SessionTimeout))
	select #tmp.ServiceID, #tmp.SourceName, #tmp.SourceID, #tmp.ServerName, #tmp.DestinationName, #tmp.LastProcessTime,
		'MaxLatency' = 
		case
			when #tmp.MaxL is null
			then cast(0 as float)
			else #tmp.MaxL
		end,
		 'RecordsLeft' =
		 case
			when TDDS_Heartbeats.RecordsLeft is null
			then 0
			else TDDS_Heartbeats.RecordsLeft
		end,
		'Throughput'=
		case 
			
			when ( ((CAST(datediff(second,  #tmp.MinTime,  #tmp.LastProcessTime)as float)) <=0) or (#tmp.Recs is null))
				then 0
			else CAST(#tmp.Recs as float) / CAST(datediff(second,  #tmp.MinTime,  #tmp.LastProcessTime)as float)
		End
	from #tmp
	inner join TDDS_Heartbeats on ( (TDDS_Heartbeats.Age = #tmp.MaxAge) and (TDDS_Heartbeats.SourceID = #tmp.SourceID) and
					(TDDS_Heartbeats.ServiceID = #tmp.ServiceID))
	
end
