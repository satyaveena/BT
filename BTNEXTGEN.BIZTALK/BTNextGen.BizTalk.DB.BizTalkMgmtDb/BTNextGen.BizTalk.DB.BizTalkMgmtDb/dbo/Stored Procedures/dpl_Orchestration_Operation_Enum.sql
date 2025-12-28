CREATE PROCEDURE [dbo].[dpl_Orchestration_Operation_Enum]
(
 @PortId as int
)

AS
set nocount on
set xact_abort on
select
 [bts_porttype_operation].[nID] as [Id],
 [bts_porttype_operation].[nvcName] as [Name],
 [bts_porttype_operation].[nType] as [OperationFlow]
from
 [bts_porttype_operation] join [bts_porttype] on ([bts_porttype_operation].[nPortTypeID] = [bts_porttype].[nID])
 join [bts_orchestration_port] on ([bts_porttype].[nID] = [bts_orchestration_port].[nPortTypeID])
where ([bts_orchestration_port].[nID] = @PortId)
order by [bts_porttype_operation].[nvcName]

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Operation_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Operation_Enum] TO [BTS_OPERATORS]
    AS [dbo];

