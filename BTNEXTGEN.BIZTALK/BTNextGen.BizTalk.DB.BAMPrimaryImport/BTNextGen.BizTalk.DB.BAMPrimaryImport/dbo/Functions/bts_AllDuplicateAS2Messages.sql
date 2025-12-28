CREATE FUNCTION bts_AllDuplicateAS2Messages()
RETURNS TABLE 
AS
	Return
	(
		select RecordID from bam_AS2MessageActivity_CompletedInstances mainTable where
		(0 < ( select count(1) from bam_AS2MessageActivity_CompletedInstances inTable 
			Where mainTable.MessageID = inTable.MessageID AND 
			mainTable.AS2From = inTable.AS2From AND mainTable.AS2To = inTable.AS2To AND
			inTable.AS2PartyRole = mainTable.AS2PartyRole AND
			(inTable.RecordID > mainTable.RecordID )
		))
	)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_AllDuplicateAS2Messages] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_AllDuplicateAS2Messages] TO [BTS_OPERATORS]
    AS [dbo];

