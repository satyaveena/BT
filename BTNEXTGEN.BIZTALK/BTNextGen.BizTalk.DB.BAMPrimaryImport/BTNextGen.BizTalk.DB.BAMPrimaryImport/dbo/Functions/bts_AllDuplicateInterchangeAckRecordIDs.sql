CREATE FUNCTION bts_AllDuplicateInterchangeAckRecordIDs()
RETURNS TABLE 
AS
	Return
	(
		select RecordID from bam_InterchangeAckActivity_CompletedInstances mainTable where
		(0 < ( select count(1) from bam_InterchangeAckActivity_CompletedInstances inTable 
			Where mainTable.InterchangeControlNo=inTable.InterchangeControlNo AND 
			mainTable.ReceiverID = inTable.ReceiverID AND mainTable.SenderID = inTable.SenderID AND 
			mainTable.ReceiverQ = inTable.ReceiverQ AND mainTable.SenderQ = inTable.SenderQ AND 
			mainTable.InterchangeDateTime = inTable.InterchangeDateTime AND 
			mainTable.Direction = inTable.Direction AND mainTable.AckProcessingStatus = inTable.AckProcessingStatus  
			AND (inTable.RecordID > mainTable.RecordID)
		))
	)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_AllDuplicateInterchangeAckRecordIDs] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_AllDuplicateInterchangeAckRecordIDs] TO [BTS_OPERATORS]
    AS [dbo];

