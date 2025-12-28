create procedure [dbo].[TDDS_GetServices]
as
set nocount on
select TDDS_Services.ServiceID, TDDS_Services.ServerName
from TDDS_Services
