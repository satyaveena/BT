CREATE PROCEDURE [dbo].[MBOM_GetHosts]
AS
	SET NOCOUNT ON
	
	select
		adm_Host.Name, 
		adm_Host.NTGroupName
	from
		adm_Host
		
	RETURN 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MBOM_GetHosts] TO [BTS_ADMIN_USERS]
    AS [dbo];

