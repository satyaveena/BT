create procedure [dbo].[TDDS_DeleteAllDestinations]
as
begin
	
	if (exists (select * from TDDS_Sources where TDDS_Sources.DestinationID in (select TDDS_Destinations.DestinationID from TDDS_Destinations)))
	begin
		declare @localized_string_error60023 as nvarchar(128)
		set @localized_string_error60023 = N'Please delete associated event sources first'
		raiserror(@localized_string_error60023, 16, 1)
		return 60023
	end
	else
	begin
		delete from TDDS_Destinations
		if (@@rowcount =0) 
		begin
		declare @localized_string_error60027 as nvarchar(128)
		set @localized_string_error60027 = N'Can Not Delete Destinations.'
		raiserror(@localized_string_error60027 ,16,1)
		return 60027
		end
		return 0
	end
end
