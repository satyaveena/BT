create procedure [dbo].[bts_ReceivePort_Timestamp_Update]
@ReceivePortId int,
@DateModified datetime output
as
set nocount on
begin tran
set @DateModified = GETUTCDATE()
UPDATE bts_receiveport SET DateModified = @DateModified where nID = @ReceivePortId
commit tran
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_ReceivePort_Timestamp_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_ReceivePort_Timestamp_Update] TO [BTS_OPERATORS]
    AS [dbo];

