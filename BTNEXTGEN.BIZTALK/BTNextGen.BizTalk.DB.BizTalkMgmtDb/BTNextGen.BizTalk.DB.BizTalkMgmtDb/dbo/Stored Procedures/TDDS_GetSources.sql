create procedure [dbo].[TDDS_GetSources]
as
set nocount on
select TDDS_Sources.SourceName, TDDS_Sources.ConnectionString, TDDS_Sources.StreamType, TDDS_Sources.AcceptableLatency,
	TDDS_Destinations.DestinationName
from TDDS_Sources inner join TDDS_Destinations on TDDS_Destinations.DestinationID=TDDS_Sources.DestinationID
