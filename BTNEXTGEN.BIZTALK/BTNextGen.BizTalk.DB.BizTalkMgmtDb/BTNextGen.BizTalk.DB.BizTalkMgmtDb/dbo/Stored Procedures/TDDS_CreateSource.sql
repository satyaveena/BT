create procedure [dbo].[TDDS_CreateSource]
(
	@DestinationName nvarchar(256),
	@SourceName nvarchar(256),
	@ConnectionString nvarchar(1024),
	@StreamType int,
	@AcceptableLatency int=null
)
as 
begin
	declare @@DestinationID uniqueidentifier
	select @@DestinationID=DestinationID from TDDS_Destinations where DestinationName=@DestinationName
	if @@DestinationID is null
	begin
		declare @localized_string_error60028 as nvarchar(128)
		set @localized_string_error60028 = N'Destination Does Not Exist.'
		raiserror(@localized_string_error60028, 16, 1)
		return 60028
	end
	if exists (select * from TDDS_Sources where (DestinationID=@@DestinationID) And (SourceName=@SourceName))
	begin
		declare @localized_string_error60008 as nvarchar(128)
		set @localized_string_error60008 = N'Source Already Exists.'
		raiserror (@localized_string_error60008,16,1)
		return 60008
	end
	else
	begin
		insert TDDS_Sources (SourceID,DestinationID,SourceName,ConnectionString,StreamType,AcceptableLatency)
		values (NEWID(),@@DestinationID,@SourceName,@ConnectionString,@StreamType,@AcceptableLatency)
		if (@@rowcount =0) 
		begin
			declare @localized_string_error60009 as nvarchar(128)
			set @localized_string_error60009 = N'Can Not Add Source.'
			raiserror(@localized_string_error60009 , 16,1)
			return 60009
		end
		
		return 0
	end
	
end
