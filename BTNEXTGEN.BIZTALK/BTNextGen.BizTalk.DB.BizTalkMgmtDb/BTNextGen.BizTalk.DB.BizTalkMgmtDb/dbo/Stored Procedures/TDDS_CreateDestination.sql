create procedure [dbo].[TDDS_CreateDestination]
(
	@DestinationName nvarchar(256),
	@ConnectionString nvarchar(1024)
)
as
begin
	
	if exists(select * from TDDS_Destinations where ((DestinationName=@DestinationName)))
	begin
		declare @localized_string_error60004 as nvarchar(128)
		set @localized_string_error60004 = N'Destination Already Exists.'
		raiserror (@localized_string_error60004,16,1)
		return 60004
	end
	
	insert into TDDS_Destinations (DestinationID,DestinationName,ConnectionString) 
	values (newid(), @DestinationName,@ConnectionString)
	if (@@rowcount =0) 
	begin
		declare @localized_string_error60005 as nvarchar(128)
		set @localized_string_error60005 = N'Can Not Add Destination.'
		raiserror(@localized_string_error60005 , 16,1)
		return 60005
	end
	return 0
end
