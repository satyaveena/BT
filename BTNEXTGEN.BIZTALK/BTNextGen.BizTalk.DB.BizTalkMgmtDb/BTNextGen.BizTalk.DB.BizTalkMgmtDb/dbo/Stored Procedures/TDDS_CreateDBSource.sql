create procedure [dbo].[TDDS_CreateDBSource]
(
	@DestinationName nvarchar(256),
	@SourceName nvarchar(256),
	@DBServerName nvarchar(256),
	@DatabaseName nvarchar(256),
	@StreamType int,
	@AcceptableLatency int=null
)
as 
begin
	declare @@ConnectionString nvarchar(1024)
	select @@ConnectionString='Pooling=false;Current Language=us_english;Integrated Security=SSPI;server='+@DBServerName+';database='+@DatabaseName
	if exists (select * from TDDS_Sources where (ConnectionString=@@ConnectionString) And (StreamType=@StreamType))
	begin
		declare @localized_string_error60039 as nvarchar(128)
		set @localized_string_error60039 = N'Source Already Exists.'
		raiserror (@localized_string_error60039,16,1)
		return 60039
	end
	else
	begin
		exec TDDS_CreateSource  @DestinationName,@SourceName,@@ConnectionString,@StreamType,@AcceptableLatency
	end
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_CreateDBSource] TO [BTS_ADMIN_USERS]
    AS [dbo];

