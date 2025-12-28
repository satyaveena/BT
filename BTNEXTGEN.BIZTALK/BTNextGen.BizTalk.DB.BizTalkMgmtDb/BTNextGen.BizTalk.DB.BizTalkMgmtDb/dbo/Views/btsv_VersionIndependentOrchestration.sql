CREATE VIEW [dbo].[btsv_VersionIndependentOrchestration] WITH SCHEMABINDING
AS
SELECT
	MAX(svc.nID) 'nOrchestrationID',
	MAX(svc.nvcFullName) 'nvcOrchestrationName',
	MAX(mod.nvcName) 'nvcAssemblyName',
	MAX(mod.nvcCulture) 'nvcAssemblyCulture',
	MAX(mod.nvcPublicKeyToken) 'nvcAssemblyPublicKeyToken',
	MAX(mod.nvcFullName) 'nvcAssemblyFullName',
	MAX(mod.dtDateModified) 'dtDateModified',
	MAX(mod.nGroupId) 'nAdminGroupId',
	MAX(svc.nAdminHostID) 'nAdminHostID',
	MAX(svc.nOrchestrationStatus) 'nOrchestrationStatus'
FROM
	[dbo].[bts_orchestration] svc,
	[dbo].[bts_assembly] mod
WHERE
	svc.nAssemblyID =  mod.nID
GROUP BY
	svc.nID
