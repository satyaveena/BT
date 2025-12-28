CREATE PROCEDURE [dbo].[edi_GetBatchOccurenceCount]
@batchId bigInt
AS

select NumOccurences from [dbo].[PAM_Batching_Log] where BatchId=@batchId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetBatchOccurenceCount] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetBatchOccurenceCount] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetBatchOccurenceCount] TO [BTS_OPERATORS]
    AS [dbo];

