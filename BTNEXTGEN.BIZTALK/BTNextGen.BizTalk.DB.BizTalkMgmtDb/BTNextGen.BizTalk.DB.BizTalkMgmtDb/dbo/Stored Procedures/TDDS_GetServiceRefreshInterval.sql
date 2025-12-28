create procedure [dbo].[TDDS_GetServiceRefreshInterval]
as
begin
	select TDDS_Settings.RefreshInterval 
	from TDDS_Settings 
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetServiceRefreshInterval] TO [BAM_CONFIG_READER]
    AS [dbo];

