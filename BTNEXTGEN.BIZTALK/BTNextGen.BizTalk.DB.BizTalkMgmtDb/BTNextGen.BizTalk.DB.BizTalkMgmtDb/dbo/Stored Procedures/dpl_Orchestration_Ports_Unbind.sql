CREATE PROCEDURE [dbo].[dpl_Orchestration_Ports_Unbind]
(
 @ModuleId as int
)

AS
set nocount on
set xact_abort on
begin tran

DELETE [bts_orchestration_port_binding]
FROM   
 [bts_orchestration_port_binding]
 INNER JOIN [bts_orchestration_port] ON ([bts_orchestration_port_binding].[nOrcPortID] = [bts_orchestration_port].[nID])
 INNER JOIN [bts_orchestration] ON ([bts_orchestration_port].[nOrchestrationID] = [bts_orchestration].[nID])
WHERE ([bts_orchestration].[nAssemblyID] = @ModuleId) 

commit tran
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Ports_Unbind] TO [BTS_ADMIN_USERS]
    AS [dbo];

