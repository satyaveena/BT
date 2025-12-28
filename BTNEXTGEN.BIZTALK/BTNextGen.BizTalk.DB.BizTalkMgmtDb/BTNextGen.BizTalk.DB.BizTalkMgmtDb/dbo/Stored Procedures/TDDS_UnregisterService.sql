create procedure [dbo].[TDDS_UnregisterService]
(
	@ServiceID uniqueidentifier
)
as
begin
	delete from TDDS_Services where (ServiceID = @ServiceID)
	if (@@rowcount = 0) 
	begin	
		declare @localized_string_error60013 as nvarchar(128)
		set @localized_string_error60013 = N'Service Does Not Exist.'
		raiserror (@localized_string_error60013,16,1)
		return 60013
	end
	return 0
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_UnregisterService] TO [BTS_ADMIN_USERS]
    AS [dbo];

