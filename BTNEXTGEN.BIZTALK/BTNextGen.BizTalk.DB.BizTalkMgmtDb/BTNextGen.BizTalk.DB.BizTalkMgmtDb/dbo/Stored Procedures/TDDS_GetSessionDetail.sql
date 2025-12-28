create procedure [dbo].[TDDS_GetSessionDetail]
(
	@SourceID uniqueidentifier
)
as
begin
	set nocount on
	if exists(select * from TDDS_Sources where SourceID=@SourceID)
	begin
		select TDDS_Services.ServerName, TDDS_Destinations.DestinationName, TDDS_Heartbeats.RecordsProcessed,
		       TDDS_Heartbeats.RecordsLeft, TDDS_Heartbeats.TimeLastChanged, TDDS_Heartbeats.Latency,
			TDDS_Heartbeats.ErrorCode, TDDS_Heartbeats.ErrorDescription
		
		from TDDS_Sources
		inner join TDDS_Heartbeats on TDDS_Heartbeats.SourceID = @SourceID
		inner join TDDS_Services on TDDS_Services.ServiceID = TDDS_Heartbeats.ServiceID
		inner join TDDS_Destinations on TDDS_Destinations.DestinationID = TDDS_Sources.DestinationID
		where TDDS_Sources.SourceID = @SourceID
		ORDER by TDDS_Services.ServerName ASC,TDDS_Destinations.DestinationName ASC, TDDS_Heartbeats.TimeLastChanged DESC
		return 0
	end
	else
	begin
		declare @localized_string_error60030 as nvarchar(128)
		set @localized_string_error60030 = N'Source Does Not Exist.'
		raiserror(@localized_string_error60030, 16, 1)
		return 60030
	end
end
