CREATE PROCEDURE [dbo].[dpl_Orchestration_Enum]
(
 @ModuleId as int
)

AS
set nocount on
set xact_abort on
select distinct
 [nID] as [Id],
 [uidGUID] as [Guid],
 [nvcNamespace] as [Namespace],
 [nvcName] as [Name],
 [nvcFullName] as [FullName],
 [nOrchestrationStatus] as [ServiceStatus],
 [adm_Host].[Name] as [HostName]
from [bts_orchestration] 
 left outer join [adm_Host] on [nAdminHostID] = [adm_Host].[Id]
where ([bts_orchestration].[nAssemblyID] = @ModuleId) 
order by [nvcFullName]
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];

