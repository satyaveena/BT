CREATE PROCEDURE [dbo].[dpl_Orchestration_Port_Enum]
(
 @ServiceId as int
)

AS
set nocount on
set xact_abort on

select
 [nID] as [Id],
 [uidGUID] as [Guid],
 [nvcName] as [Name],
 [nPolarity] as [Polarity],
 [nBindingOption] as [BindingOption]
from [bts_orchestration_port]
where ([nOrchestrationID] = @ServiceId)
order by [nvcName]

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Port_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Port_Enum] TO [BTS_OPERATORS]
    AS [dbo];

