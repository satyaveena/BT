create procedure [dbo].[TDDS_GetCustomFormats]
as 
set nocount on
select * from TDDS_CustomFormats

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetCustomFormats] TO [BAM_CONFIG_READER]
    AS [dbo];

