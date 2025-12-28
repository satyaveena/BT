CREATE FUNCTION bts_AllDuplicateAS2IsaMessages()
RETURNS TABLE 
AS
	Return
	(
		select RecordID from bam_AS2InterchangeActivity_CompletedInstances mainTable where
		(0 < ( select count(1) from bam_AS2InterchangeActivity_CompletedInstances inTable 
			Where mainTable.MessageID = inTable.MessageID AND 
			mainTable.AS2From = inTable.AS2From AND mainTable.AS2To = inTable.AS2To
			AND (inTable.RecordID > mainTable.RecordID )
		))
	)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_AllDuplicateAS2IsaMessages] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_AllDuplicateAS2IsaMessages] TO [BTS_OPERATORS]
    AS [dbo];

