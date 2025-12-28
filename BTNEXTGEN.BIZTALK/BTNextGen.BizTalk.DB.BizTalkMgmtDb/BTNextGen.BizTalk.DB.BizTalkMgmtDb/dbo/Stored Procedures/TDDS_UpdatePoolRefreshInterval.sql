create procedure [dbo].[TDDS_UpdatePoolRefreshInterval]
(	
	@RefreshInterval int
)
as
begin
	update TDDS_Settings set RefreshInterval = @RefreshInterval
	if (@@rowcount =0) 
	begin
		declare @localized_string_error60018 as nvarchar(128)
		set @localized_string_error60018 = N'Can Not Update  Refresh Interval.'
		raiserror(@localized_string_error60018, 16, 1)
		return 60018
	end
	return 0
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_UpdatePoolRefreshInterval] TO [BTS_ADMIN_USERS]
    AS [dbo];

