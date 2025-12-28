create procedure [dbo].[bts_SendPortGroup_Status_Update]
@SendPortGroupId int,
@Status int,
@DateModified datetime,
@DateModifiedOut datetime output
as
begin tran
set @DateModifiedOut = GETUTCDATE()
UPDATE bts_sendportgroup SET nPortStatus = @Status, DateModified = @DateModifiedOut 
WHERE nID = @SendPortGroupId AND DateModified = @DateModified
commit tran

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_SendPortGroup_Status_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_SendPortGroup_Status_Update] TO [BTS_OPERATORS]
    AS [dbo];

