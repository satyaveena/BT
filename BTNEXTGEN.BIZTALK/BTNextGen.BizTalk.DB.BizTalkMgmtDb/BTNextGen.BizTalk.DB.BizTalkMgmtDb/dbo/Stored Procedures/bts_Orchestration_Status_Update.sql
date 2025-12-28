create procedure [dbo].[bts_Orchestration_Status_Update]
@OrchestrationId int,
@Status int,
@DateModified datetime,
@DateModifiedOut datetime output
as
begin tran
set @DateModifiedOut = GETUTCDATE()
UPDATE bts_orchestration  SET nOrchestrationStatus = @Status, dtModified = @DateModifiedOut 
WHERE nID = @OrchestrationId AND dtModified = @DateModified
commit tran

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_Orchestration_Status_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_Orchestration_Status_Update] TO [BTS_OPERATORS]
    AS [dbo];

