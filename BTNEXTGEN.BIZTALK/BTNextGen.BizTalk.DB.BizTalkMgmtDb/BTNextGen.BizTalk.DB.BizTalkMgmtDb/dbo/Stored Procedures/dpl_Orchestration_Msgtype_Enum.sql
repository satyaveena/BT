CREATE PROCEDURE [dbo].[dpl_Orchestration_Msgtype_Enum]
(
 @OperationId as int
)

AS
set nocount on
set xact_abort on

select
 [nID] as [Id],
 [nvcName] as [Name],
 [nMsgType] as [MsgType]
from [bts_service_msgtype]
where ([nOperationID] = @OperationId)
order by [nvcName]

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Msgtype_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];

