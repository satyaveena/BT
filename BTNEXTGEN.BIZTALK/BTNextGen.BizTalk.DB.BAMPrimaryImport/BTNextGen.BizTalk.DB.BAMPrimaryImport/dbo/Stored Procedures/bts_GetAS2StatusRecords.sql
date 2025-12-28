CREATE  PROCEDURE [dbo].[bts_GetAS2StatusRecords]
(
	@receiverPartyName 			nvarchar(256) 	= NULL,
	@senderPartyName 			nvarchar(256) 	= NULL,
	@as2MessageID 				nvarchar(1000) 	= NULL,
	@as2PartyRole 				int 			= NULL,
	@mdnProcessingStatus 		int 			= NULL,
	@useStatusEqualsOperator 	bit 			= 1,
	@messageStartDateTime 		datetime 		= NULL,
	@messageEndDateTime 		datetime 		= NULL,
	@maxRecords 				int 			= NULL,
	@debug 						bit 			= 0
) AS
BEGIN 
	if @maxRecords is NOT NULL 
		SET ROWCOUNT @maxRecords

	SELECT 
		mesgTable.ReceiverPartyName, 
		mesgTable.SenderPartyName, 
		mesgTable.AS2PartyRole, 
		mesgTable.AS2From, 
		mesgTable.AS2To, 
		mesgTable.MessageID, 
		mesgTable.MessageDateTime, 
		mesgTable.TimeCreated, 
		mesgTable.MessageEncryptionType, 
		mesgTable.MdnProcessingStatus, 
		mesgTable.IsMdnExpected, 
		mesgTable.MessageSignatureType, 
		mesgTable.MessagePayloadContentKey, 
		mesgTable.MessageWireContentKey, 
		mesgTable.MessageMicValue,
		mesgTable.IsAS2MessageDuplicate,
		mesgTable.DaysToCheckDuplicate,
		mesgTable.Filename,
		mesgTable.AgreementName,
		isaTable.ReceiverID, 
		isaTable.SenderID, 
		isaTable.ReceiverQ, 
		isaTable.SenderQ, 
		isaTable.InterchangeControlNo, 
		isaTable.InterchangeDateTime,
		mdnTable.MdnDispositionType, 
		mdnTable.DispositionModifierExtType, 
		mdnTable.DispositionModifierExtDescription,
		mdnTable.MdnPayloadContentKey, 
		mdnTable.MdnWireContentKey,
		resendTable.ResendCount,
		resendTable.MaxResendCount,
		resendTable.ResendInterval,
		resendTable.MaxRetryCount,
		resendTable.RetryInterval,
		resendTable.ResendTimeout,
		resendTable.RetryTimeout,
		resendTable.ActivityID,
		journalTable.ResendStatus  	FROM 
		bam_AS2MessageActivity_CompletedInstances 						AS mesgTable
		LEFT OUTER JOIN bam_AS2InterchangeActivity_CompletedInstances 	AS isaTable ON
			mesgTable.AS2From = isaTable.AS2From 
			AND mesgTable.AS2To = isaTable.AS2To 
			AND mesgTable.MessageID = isaTable.MessageID 
			AND isaTable.RecordID Not in (select RecordID  from bts_AllDuplicateAS2IsaMessages())
		LEFT OUTER JOIN bam_AS2MdnActivity_CompletedInstances 			AS mdnTable ON
			mesgTable.AS2From = mdnTable.AS2To 
			AND mesgTable.AS2To = mdnTable.AS2From 
			AND mesgTable.MessageID = mdnTable.MessageID 
		 	AND mesgTable.AS2PartyRole <> mdnTable.AS2PartyRole 
			AND mdnTable.RecordID Not in (select RecordID from bts_AllDuplicateMdnMessages() )
		LEFT OUTER JOIN bam_ResendTrackingActivity_AllInstances			AS resendTable ON
			resendTable.BTSInterchangeID = mesgTable.BTSInterchangeID
		LEFT OUTER JOIN bam_ResendJournalActivity_AllInstances			AS journalTable ON
				journalTable.TrackingActivityID = resendTable.ActivityID
			AND	(journalTable.ResendStatus = 'ResendAttemptsExhausted'
				OR journalTable.ResendStatus = 'ResendTimeout')
	WHERE 
		mesgTable.RecordID Not in (select RecordID from bts_AllDuplicateAS2Messages())
		AND (mesgTable.ReceiverPartyName = @receiverPartyName 	OR @receiverPartyName IS NULL)
		AND (mesgTable.SenderPartyName = @senderPartyName 	OR @senderPartyName IS NULL)
		AND (mesgTable.MessageID = @as2MessageID 		OR @as2MessageID IS NULL)
		AND (mesgTable.AS2PartyRole = @as2PartyRole 		OR @as2PartyRole IS NULL)
		AND (	@mdnProcessingStatus IS NOT NULL
				AND ((@useStatusEqualsOperator = 0
					AND (	(mesgTable.MdnProcessingStatus <> @mdnProcessingStatus AND mdnTable.MdnDispositionType IS NULL) 
					OR mdnTable.MdnDispositionType <> @mdnProcessingStatus))
				      OR  (@useStatusEqualsOperator <> 0
					AND ((mesgTable.MdnProcessingStatus = @mdnProcessingStatus AND mdnTable.MdnDispositionType IS NULL) 
					OR mdnTable.MdnDispositionType = @mdnProcessingStatus))
				    )
			OR
			@mdnProcessingStatus IS NULL)
		AND (mesgTable.MessageDateTime >= @messageStartDateTime OR @messageStartDateTime IS NULL)
		AND (mesgTable.MessageDateTime <= @messageEndDateTime 	OR @messageEndDateTime IS NULL)
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetAS2StatusRecords] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetAS2StatusRecords] TO [BTS_OPERATORS]
    AS [dbo];

