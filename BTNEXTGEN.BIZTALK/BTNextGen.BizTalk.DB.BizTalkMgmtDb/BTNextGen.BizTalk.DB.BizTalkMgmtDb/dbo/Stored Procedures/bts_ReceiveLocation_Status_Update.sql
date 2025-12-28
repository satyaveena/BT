create procedure [dbo].[bts_ReceiveLocation_Status_Update]
@ReceiveLocationId int,
@Status int,
@DateModified datetime,
@DateModifiedOut datetime output
as
begin tran
set @DateModifiedOut = GETUTCDATE()
UPDATE adm_ReceiveLocation SET Disabled = @Status, DateModified = @DateModifiedOut 
WHERE Id = @ReceiveLocationId AND DateModified = @DateModified
commit tran

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_ReceiveLocation_Status_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_ReceiveLocation_Status_Update] TO [BTS_OPERATORS]
    AS [dbo];

