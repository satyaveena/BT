create procedure [dbo].[TDDS_UpdateSettings]
(
 	@RefreshInterval int ,
	@SqlCommandTimeout int ,
	@SessionTimeout int ,
	@EventLoggingInterval nvarchar(16),
	@RetryCount int ,
	@ThreadPerSession int 
)
as
begin
	if (@ThreadPerSession<=0)
	begin
		declare @localized_string_error61110 as nvarchar(128)
		set @localized_string_error61110 = N'Threads per session has to be set to a value grater than 0'
		raiserror(@localized_string_error61110, 16, 1)
		return 61110
	end
	if (@RefreshInterval<60)
	begin
		declare @localized_string_error61120 as nvarchar(128)
		set @localized_string_error61120 = N'Refresh interval must be greater than or equal to 60'
		raiserror(@localized_string_error61120, 16, 1)
		return 61120
	end
	if ((@SqlCommandTimeout <@RefreshInterval) And (@SessionTimeout >2*@RefreshInterval))
	begin
		UPDATE TDDS_Settings
		set @RefreshInterval=@RefreshInterval,@SqlCommandTimeout=@SqlCommandTimeout,
		  @SessionTimeout=@SessionTimeout,@EventLoggingInterval=@EventLoggingInterval,
		  @RetryCount=@RetryCount, @ThreadPerSession=@ThreadPerSession
		
	end
	else
	begin
		declare @localized_string_error61130 as nvarchar(128)
		set @localized_string_error61130 = N'The following condition has not been satisfied:(SqlCommandTimeout < RefreshInterval) And (SessionTimeout > 2 * RefreshInterval) '
		raiserror(@localized_string_error61130, 16, 1)
		return 61130
	end
	return 0
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_UpdateSettings] TO [BTS_ADMIN_USERS]
    AS [dbo];

