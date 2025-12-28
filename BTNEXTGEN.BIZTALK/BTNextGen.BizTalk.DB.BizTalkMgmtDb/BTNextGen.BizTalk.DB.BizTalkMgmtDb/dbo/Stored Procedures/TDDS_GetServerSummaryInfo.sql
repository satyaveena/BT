create procedure [dbo].[TDDS_GetServerSummaryInfo]
as
begin
	set nocount on
	declare @@SessionTimeout int
	select @@SessionTimeout=SessionTimeout from TDDS_Settings
	select distinct TDDS_Services.ServerName,TDDS_Services.ServiceID,MAX(TDDS_Heartbeats.TimeLastChanged) as 'LastProcessTime',
	        COUNT(DISTINCT Cast(TDDS_Heartbeats.SourceID AS nvarchar(256))) as N'ActiveSessions',
		'Throughput'=
		case 			
			when ( ((CAST(datediff(second,  MIN(TDDS_Heartbeats.TimeLastChanged),  MAX(TDDS_Heartbeats.TimeLastChanged))as float)) <=0) Or (SUM(TDDS_Heartbeats.RecordsProcessed) is null))
				then 0
			else CAST(SUM(TDDS_Heartbeats.RecordsProcessed) as float) / CAST(datediff(second,  MIN(TDDS_Heartbeats.TimeLastChanged),  MAX(TDDS_Heartbeats.TimeLastChanged))as float)
		End
	from TDDS_Services
	inner join TDDS_Heartbeats on TDDS_Services.ServiceID = TDDS_Heartbeats.ServiceID
	where (TDDS_Heartbeats.SourceID is not null)
	Group BY TDDS_Services.ServiceID, TDDS_Services.ServerName
	HAVING (datediff(second, MAX(TDDS_Heartbeats.TimeLastChanged), GETUTCDATE()) < (@@SessionTimeout))
end
