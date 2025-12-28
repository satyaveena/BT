CREATE PROCEDURE [dbo].[bts_MarkAndGetDuplicateInterchangeAckRecords] 
AS
BEGIN	
	Begin Transaction Tx1

		select Count(*)-1 NumDuplicates , InterchangeControlNo, ReceiverID, SenderID, ReceiverQ, SenderQ, 
			InterchangeDateTime, Direction from bam_InterchangeAckActivity_Completed 
		Where (RowFlags & 0x1) = 0
		Group By InterchangeControlNo, ReceiverID, SenderID, ReceiverQ, SenderQ, InterchangeDateTime, Direction, AckProcessingStatus
		Having Count(*) > 1
		
		Update bam_InterchangeAckActivity_Completed 
		Set RowFlags = RowFlags | 0x1  FROM bam_InterchangeAckActivity_Completed  mainTable
		Where (mainTable.RowFlags & 0x1) = 0 AND 
			(0 < ( select count(1) from bam_InterchangeAckActivity_Completed inTable 
				Where mainTable.InterchangeControlNo=inTable.InterchangeControlNo AND 
					mainTable.ReceiverID = inTable.ReceiverID AND mainTable.SenderID = inTable.SenderID AND 
					mainTable.ReceiverQ = inTable.ReceiverQ AND mainTable.SenderQ = inTable.SenderQ AND 
					mainTable.InterchangeDateTime = inTable.InterchangeDateTime AND 
					mainTable.Direction = inTable.Direction AND mainTable.AckProcessingStatus = inTable.AckProcessingStatus  
				AND ( inTable.TimeCreated < mainTable.TimeCreated OR inTable.RecordID < mainTable.RecordID ) ) )
	Commit Transaction Tx1
END