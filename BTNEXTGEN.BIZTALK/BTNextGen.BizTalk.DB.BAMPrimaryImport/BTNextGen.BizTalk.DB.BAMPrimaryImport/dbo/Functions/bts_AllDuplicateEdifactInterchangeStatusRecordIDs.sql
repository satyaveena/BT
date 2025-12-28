CREATE FUNCTION bts_AllDuplicateEdifactInterchangeStatusRecordIDs()
RETURNS TABLE 
AS
	Return
	(
		select RecordID from bam_InterchangeStatusActivity_CompletedInstances mainTable where
		(0 < ( select count(1) from bam_InterchangeStatusActivity_CompletedInstances inTable 
			Where mainTable.InterchangeControlNo=inTable.InterchangeControlNo AND 
			mainTable.ReceiverID = inTable.ReceiverID AND mainTable.SenderID = inTable.SenderID AND 
			mainTable.ReceiverQ = inTable.ReceiverQ AND mainTable.SenderQ = inTable.SenderQ AND 
			mainTable.Direction = inTable.Direction  AND mainTable.EdiMessageType=1
			AND (inTable.RecordID > mainTable.RecordID)
		)) 
	)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_AllDuplicateEdifactInterchangeStatusRecordIDs] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_AllDuplicateEdifactInterchangeStatusRecordIDs] TO [BTS_OPERATORS]
    AS [dbo];

