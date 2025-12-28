create procedure [dbo].[TDDS_GetServerDetail]
(
	@ServiceID uniqueidentifier
)
as
begin
	declare @@ID nvarchar(128)
	set nocount on
	select @@ID=CAST ( @ServiceID as nvarchar(128))
	if exists(select * from TDDS_Services where ServiceID = @ServiceID)
	begin
		select TDDS_Sources.SourceName, TDDS_Destinations.DestinationName, TDDS_Heartbeats.RecordsProcessed,
			TDDS_Heartbeats.RecordsLeft, TDDS_Heartbeats.TimeLastChanged, TDDS_Heartbeats.Latency,
			TDDS_Heartbeats.ErrorCode, TDDS_Heartbeats.ErrorDescription
		from TDDS_Services
		inner join TDDS_Heartbeats on TDDS_Heartbeats.ServiceID = TDDS_Services.ServiceID
		inner join TDDS_Sources on TDDS_Sources.SourceID = TDDS_Heartbeats.SourceID
		inner join TDDS_Destinations on TDDS_Destinations.DestinationID = TDDS_Sources.DestinationID
		
		where TDDS_Services.ServiceID = @ServiceID
		ORDER by TDDS_Sources.SourceName ASC, TDDS_Destinations.DestinationName ASC, TDDS_Heartbeats.TimeLastChanged DESC
		return 0
	end
	else
	begin
		declare @localized_string_error60019 as nvarchar(128)
		set @localized_string_error60019 = N'Service Does Not Exist.'
		raiserror(@localized_string_error60019, 16, 1)
		return 60019
	end
end
