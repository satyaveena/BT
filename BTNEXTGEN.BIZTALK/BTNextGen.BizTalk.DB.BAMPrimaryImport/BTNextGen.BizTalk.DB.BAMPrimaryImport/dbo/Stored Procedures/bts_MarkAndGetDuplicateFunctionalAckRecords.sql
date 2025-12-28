CREATE PROCEDURE [dbo].[bts_MarkAndGetDuplicateFunctionalAckRecords]
AS
BEGIN	
	Begin Transaction Tx1

				select Count(*)-1 NumDuplicates , GroupControlNo, ReceiverID, SenderID, ReceiverQ, SenderQ, 
			Direction from bam_FunctionalAckActivity_Completed 
		Where (RowFlags & 0x1) = 0
		Group By GroupControlNo, ReceiverID, SenderID, ReceiverQ, SenderQ, InterchangeDateTime, Direction, AckProcessingStatus
		Having Count(*) > 1
		
		Update bam_FunctionalAckActivity_Completed 
		Set RowFlags = RowFlags | 0x1  FROM bam_FunctionalAckActivity_Completed mainTable
		Where (mainTable.RowFlags & 0x1) = 0 AND 
			(0 < ( select count(1) from bam_FunctionalAckActivity_Completed inTable Where mainTable.GroupControlNo=inTable.GroupControlNo AND 
				mainTable.ReceiverID = inTable.ReceiverID AND mainTable.SenderID = inTable.SenderID AND 
				mainTable.ReceiverQ = inTable.ReceiverQ AND mainTable.SenderQ = inTable.SenderQ AND 
				mainTable.Direction = inTable.Direction AND mainTable.AckProcessingStatus = inTable.AckProcessingStatus  
				AND ( inTable.TimeCreated < mainTable.TimeCreated OR inTable.RecordID < mainTable.RecordID ) ) )
		
	Commit Transaction Tx1
END