create procedure [dbo].[TDDS_GetHeartbeats]
as
set nocount on
 select TDDS_Heartbeats.TimeLastChanged, TDDS_Heartbeats.RecordsProcessed, TDDS_Heartbeats.RecordsLeft, TDDS_Heartbeats.Latency,
	TDDS_Heartbeats.ErrorCode, TDDS_Heartbeats.ErrorDescription, TDDS_Sources.SourceName, TDDS_Services.ServiceID,TDDS_Services.ServerName
 from TDDS_Heartbeats 
 inner join TDDS_Sources on TDDS_Sources.SourceID=TDDS_Heartbeats.SourceID 
 inner join TDDS_Services on TDDS_Services.ServiceID=TDDS_Heartbeats.ServiceID
