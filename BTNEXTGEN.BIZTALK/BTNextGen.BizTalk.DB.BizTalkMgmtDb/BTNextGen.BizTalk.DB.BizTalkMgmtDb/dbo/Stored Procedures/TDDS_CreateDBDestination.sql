create procedure [dbo].[TDDS_CreateDBDestination]
(
	@DestinationName nvarchar(256),
	@DBServerName nvarchar(256),
	@DatabaseName nvarchar(256)
)
as
begin
	declare @@ConnectionString nvarchar(1024)
	select @@ConnectionString='Pooling=false;Current Language=us_english;Integrated Security=SSPI;server='+@DBServerName+';database='+@DatabaseName
	exec TDDS_CreateDestination @DestinationName,@@ConnectionString
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_CreateDBDestination] TO [BTS_ADMIN_USERS]
    AS [dbo];

