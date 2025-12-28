CREATE PROCEDURE [dbo].[bts_MarkAndGetDuplicateInterchangeStatusRecords]
AS
BEGIN	
	Begin Transaction Tx1

		select Count(*)-1 NumDuplicates , InterchangeControlNo, ReceiverID, SenderID, ReceiverQ, SenderQ, 
			InterchangeDateTime, Direction from bam_InterchangeStatusActivity_Completed 
		Where (RowFlags & 0x1) = 0
		Group By InterchangeControlNo, ReceiverID, SenderID, ReceiverQ, SenderQ, InterchangeDateTime, Direction
		Having Count(*) > 1
		
		Update bam_InterchangeStatusActivity_Completed 
		Set RowFlags = RowFlags | 0x1  FROM bam_InterchangeStatusActivity_Completed  mainTable
		Where (mainTable.RowFlags & 0x1) = 0 AND 
			(0 < ( select count(1) from bam_InterchangeStatusActivity_Completed inTable Where 
				mainTable.InterchangeControlNo=inTable.InterchangeControlNo AND 
				mainTable.ReceiverID = inTable.ReceiverID AND mainTable.SenderID = inTable.SenderID AND 
				mainTable.ReceiverQ = inTable.ReceiverQ AND mainTable.SenderQ = inTable.SenderQ AND 
				mainTable.InterchangeDateTime = inTable.InterchangeDateTime AND mainTable.Direction = inTable.Direction 
				AND ( inTable.TimeCreated < mainTable.TimeCreated OR inTable.RecordID < mainTable.RecordID ) ) )
		
	Commit Transaction Tx1
END