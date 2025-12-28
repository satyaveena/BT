CREATE PROCEDURE [dbo].[edi_GetOrchestrationInstanceForParty]
@BatchId bigint
AS

select BatchOrchestrationId from [dbo].[PAM_Batching_Log] where BatchId=@BatchId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetOrchestrationInstanceForParty] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetOrchestrationInstanceForParty] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetOrchestrationInstanceForParty] TO [BTS_OPERATORS]
    AS [dbo];

