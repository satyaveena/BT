CREATE PROCEDURE [dbo].[edi_CreateControlMessage_Override] (
	@BatchId bigint,
	@EdiMessageType smallint,
	@BatchName nvarchar (256),
	@ReceiverPartyName nvarchar (256),
	@SenderPartyName nvarchar (256),
	@AgreementName nvarchar (256)
)

AS

Declare @Status bit


if exists (Select * from [dbo].[PAM_Control] Where [BatchId] = @BatchId and [UsedOnce] = 0)
Begin
	Select @Status = 0
	Select @Status as Status
	Return
End

INSERT INTO [dbo].[PAM_Control] (
			[EdiMessageType],
			[ActionType],
			[ActionDateTime],
			[UsedOnce],
			[BatchId],
			[BatchName],
			[SenderPartyName],
			[ReceiverPartyName],
			[AgreementName] 
		) VALUES (
			@EdiMessageType,
			'EdiBatchOverride',
			GetDate(),
			0,
			@BatchId,
			@BatchName,
			@SenderPartyName,
			@ReceiverPartyName,
			@AgreementName
		)

Select @Status = 1
Select @Status as Status
Return
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CreateControlMessage_Override] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CreateControlMessage_Override] TO [BTS_OPERATORS]
    AS [dbo];

