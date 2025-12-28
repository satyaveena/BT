create procedure [dbo].[TDDS_GetDestinations]
as
set nocount on
select TDDS_Destinations.DestinationName, TDDS_Destinations.ConnectionString
from TDDS_Destinations
