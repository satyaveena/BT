create procedure [dbo].[TDDS_GetSettings]
as
begin
	select * 
	from TDDS_Settings 
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetSettings] TO [BAM_CONFIG_READER]
    AS [dbo];

