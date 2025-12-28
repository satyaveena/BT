create procedure [dbo].[bts_SendPort_Status_Update]
@SendPortId int,
@Status int,
@DateModified datetime,
@DateModifiedOut datetime output
as
begin tran
set @DateModifiedOut = GETUTCDATE()
UPDATE bts_sendport SET nPortStatus = @Status, DateModified = @DateModifiedOut 
WHERE nID = @SendPortId AND DateModified = @DateModified
commit tran

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_SendPort_Status_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_SendPort_Status_Update] TO [BTS_OPERATORS]
    AS [dbo];

