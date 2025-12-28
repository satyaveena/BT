CREATE PROCEDURE [dbo].[ops_LoadOrchestrationServiceNames]
AS

set transaction isolation level read committed
set nocount on
set deadlock_priority low

SELECT orch.uidGUID, orch.nvcFullName + N', ' + a.nvcFullName 
FROM bts_orchestration orch WITH (ROWLOCK READPAST)
left outer join bts_assembly a WITH (ROWLOCK READPAST)
ON orch.nAssemblyID = a.nID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ops_LoadOrchestrationServiceNames] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ops_LoadOrchestrationServiceNames] TO [BTS_OPERATORS]
    AS [dbo];

