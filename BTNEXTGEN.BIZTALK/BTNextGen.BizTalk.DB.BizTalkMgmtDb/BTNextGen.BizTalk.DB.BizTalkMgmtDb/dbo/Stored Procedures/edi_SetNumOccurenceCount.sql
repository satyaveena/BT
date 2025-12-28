CREATE PROCEDURE [dbo].[edi_SetNumOccurenceCount]
@batchId bigint,
@cnt int
AS

update [dbo].[PAM_Batching_Log] set NumOccurences=@cnt where BatchId=@batchId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_SetNumOccurenceCount] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_SetNumOccurenceCount] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_SetNumOccurenceCount] TO [BTS_B2B_OPERATORS]
    AS [dbo];

