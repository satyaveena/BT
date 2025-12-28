create procedure [dbo].[TDDS_DeleteAllSources]
(
	@DestinationName nvarchar(256)
)
as
begin
	declare @@DestID uniqueidentifier
	select @@DestID = DestinationID from TDDS_Destinations where DestinationName = @DestinationName
	
	if (@@DestID is null)
	begin
		declare @localized_string_error60029 as nvarchar(128)
		set @localized_string_error60029 = N'Destination Does Not Exist.'
		raiserror(@localized_string_error60029, 16, 1)
		return 60029
	end
	else
	begin
		delete from TDDS_Sources where DestinationID=@@DestID
		if (@@rowcount =0) 
		begin
			declare @localized_string_error60031 as nvarchar(128)
			set @localized_string_error60031 = N'Can Not Delete Source.'
			raiserror(@localized_string_error60031 ,16,1)
			return 60031
		end
		return 0
	end
end
