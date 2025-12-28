CREATE PROCEDURE [dbo].[edi_IsControlMessagePending] (
	@BatchId bigint
)

AS

Declare @Status bit

if exists (Select * from [dbo].[PAM_Control] WHERE [BatchId] = @BatchId and [UsedOnce] = 0)
Begin
	Select @Status = 1
	Select @Status as Status
	return
End

Select @Status = 0
Select @Status as Status
return
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_IsControlMessagePending] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_IsControlMessagePending] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_IsControlMessagePending] TO [BTS_OPERATORS]
    AS [dbo];

