create procedure [dbo].[TDDS_DeleteSource]
(
	@DestinationName nvarchar(256),
	@SourceName	nvarchar(256)
)
as
begin
	declare @@DestinationID uniqueidentifier
	select @@DestinationID=DestinationID from TDDS_Destinations where DestinationName=@DestinationName
	if @@DestinationID is null
	begin
		declare @localized_string_error60007 as nvarchar(128)
		set @localized_string_error60007 = N'Destination Does Not Exist.'
		raiserror(@localized_string_error60007, 16, 1)
		return 60007
	end
	if exists(select * from TDDS_Sources where (DestinationID=@@DestinationID) And (SourceName=@SourceName))
	begin
		delete from TDDS_Sources where (DestinationID=@@DestinationID) And (SourceName=@SourceName)
		if (@@rowcount =0) 
		begin
			declare @localized_string_error60010 as nvarchar(128)
			set @localized_string_error60010 = N'Can Not Delete Source.'
			raiserror(@localized_string_error60010 ,16,1)
			return 60010
		end
		return 0
	end
	else
	begin
		declare @localized_string_error60011 as nvarchar(128)
		set @localized_string_error60011 = N'Source Does Not Exist.'
		raiserror(@localized_string_error60011, 16, 1)
		return 60011
	end
	
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_DeleteSource] TO [BTS_ADMIN_USERS]
    AS [dbo];

