CREATE PROCEDURE [dbo].[edi_UpdateOrchestrationInstance]
@batchId bigint,
@instanceId uniqueidentifier
AS

if exists (select * from [dbo].[PAM_Batching_Log] where BatchId = @batchId)
	update [dbo].[PAM_Batching_Log] set BatchOrchestrationId=@instanceId where BatchId = @batchId
else
	Begin
	insert into [dbo].[PAM_Batching_Log] (BatchId, BatchOrchestrationId) values (@batchId, @instanceId)
	End
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_UpdateOrchestrationInstance] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_UpdateOrchestrationInstance] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_UpdateOrchestrationInstance] TO [BTS_B2B_OPERATORS]
    AS [dbo];

