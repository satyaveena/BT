create procedure [dbo].[TDDS_DeleteDestination]
(	
	@DestinationName	nvarchar(256)
)
as
begin
	declare @localized_string_error60006 as nvarchar(128)
	set @localized_string_error60006 = N'Can Not Delete Destination.'
	declare @@DestinationID uniqueidentifier
	
	select @@DestinationID=DestinationID 
	from   TDDS_Destinations 
	where (DestinationName=@DestinationName)
	if @@DestinationID is null
	begin
		declare @localized_string_error60026 as nvarchar(128)
		set @localized_string_error60026 = N'Destination Does Not Exist.'
		raiserror(@localized_string_error60026, 16, 1)
		return 60026
	end
	else
	begin
		if exists(select * from TDDS_Sources where (DestinationID=@@DestinationID))
		begin
			raiserror(@localized_string_error60006 ,16,1)
			return 60006
		end
		else
		begin
			delete  from TDDS_Destinations where (DestinationName=@DestinationName)
			if (@@rowcount =0) 
			begin		
				raiserror(@localized_string_error60006, 16, 1)
				return 60006
			end
			
			return 0
		end
		
	end
	
end
