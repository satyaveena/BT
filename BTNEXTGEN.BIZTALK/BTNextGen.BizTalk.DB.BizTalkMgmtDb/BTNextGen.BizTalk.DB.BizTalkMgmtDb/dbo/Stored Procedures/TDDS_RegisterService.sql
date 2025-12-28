create procedure [dbo].[TDDS_RegisterService]
(
	@ServiceID uniqueidentifier,
	@ServerName nvarchar(32)
)
as
begin
	declare @@ID nvarchar(128)
	
	if exists (select * from TDDS_Services where ServiceID = @ServiceID)
	begin		
		declare @localized_string_error60012 as nvarchar(128)
		set @localized_string_error60012 = N'Already Register Service.'
		raiserror (@localized_string_error60012,16,1)
		return 60012
	end
	insert TDDS_Services (ServiceID,ServerName)
	values(@ServiceID,@ServerName)
	return 0
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_RegisterService] TO [BTS_ADMIN_USERS]
    AS [dbo];

