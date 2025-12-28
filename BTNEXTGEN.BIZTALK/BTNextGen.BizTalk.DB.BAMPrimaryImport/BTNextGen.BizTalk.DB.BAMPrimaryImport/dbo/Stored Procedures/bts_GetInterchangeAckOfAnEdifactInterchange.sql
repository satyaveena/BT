CREATE PROCEDURE [dbo].[bts_GetInterchangeAckOfAnEdifactInterchange]
(
	@receiverID nvarchar(35), @receiverQ nvarchar(4),
	@senderID nvarchar(35), @senderQ nvarchar(4),
	@interchangeControlNo nvarchar(14),
	@direction int
) AS
BEGIN
	if (@direction = 2) 	begin
		select TOP 1 AckIcn, ReceiverID, SenderID, ReceiverQ, SenderQ, Direction, AckIcnDate, 
			AckIcnTime, AckProcessingStatus, AckStatusCode, AckNoteCode1, AckNoteCode2, [TimeCreated]  
		From bam_InterchangeAckActivity_CompletedInstances
		where ReceiverID = @senderID AND SenderID = @receiverID AND 
			ReceiverQ = @senderQ AND SenderQ = @receiverQ AND 
			InterchangeControlNo = @interchangeControlNo AND Direction=1
		order by TimeCreated desc
	end
	else		begin
		select TOP 1 AckIcn, ReceiverID, SenderID, ReceiverQ, SenderQ, Direction, AckIcnDate, AckIcnTime, 
			AckProcessingStatus, AckStatusCode, AckNoteCode1, AckNoteCode2, [TimeCreated]  
		From bam_InterchangeAckActivity_CompletedInstances
		where ReceiverID = @senderID AND SenderID = @receiverID AND 
			ReceiverQ = @senderQ AND SenderQ = @receiverQ AND InterchangeControlNo = @interchangeControlNo
			AND Direction=2
		order by TimeCreated desc
	end
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetInterchangeAckOfAnEdifactInterchange] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetInterchangeAckOfAnEdifactInterchange] TO [BTS_OPERATORS]
    AS [dbo];

