CREATE PROCEDURE [dbo].[MBOM_GetOrchestrationTypes]
AS
	SET NOCOUNT ON
	
	select distinct
		o.nvcName,
		o.uidGUID,
		a.nvcName,
		a.nvcVersion,
		a.nvcCulture,
		a.nvcPublicKeyToken,
		h.Name
	from
		bts_assembly a,
		bts_orchestration o left outer join adm_Host h on o.nAdminHostID = h.Id
	where
		o.nAssemblyID = a.nID
		
		
	RETURN 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MBOM_GetOrchestrationTypes] TO [BTS_ADMIN_USERS]
    AS [dbo];

