CREATE PROCEDURE [dbo].[edi_PAMBatchingLogDelete] (
	@BatchId bigint,
	@IgnorePendingControlMessages bit
)

AS

Declare @Status bit

if (@IgnorePendingControlMessages = 0)
Begin
	if exists (Select * from [dbo].[PAM_Control] WHERE [BatchId] = @BatchId and [UsedOnce] = 0)
	Begin
		Select @Status = 0
		Select @Status as Status
		return
	End
End

Delete from [dbo].[PAM_Batching_Log] Where [BatchId] = @BatchId

Select @Status = 1
Select @Status as Status
return
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_PAMBatchingLogDelete] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_PAMBatchingLogDelete] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_PAMBatchingLogDelete] TO [BTS_B2B_OPERATORS]
    AS [dbo];

