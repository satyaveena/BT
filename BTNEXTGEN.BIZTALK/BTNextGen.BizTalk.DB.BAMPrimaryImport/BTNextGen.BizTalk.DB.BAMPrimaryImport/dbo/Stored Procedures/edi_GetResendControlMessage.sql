CREATE        Procedure [dbo].[edi_GetResendControlMessage]
AS
BEGIN

BEGIN TRANSACTION

	SELECT	
			1														as IsResendControlMessage,
			Resend_Control.ActivityID								as TrackingActivityID,
			Resend_Control.AdapterPrefix							as AdapterPrefix,
			Resend_Control.CorrelationId							as CorrelationId,
			Resend_Control.MessageContentId 						as ContentId,
			Journal.ResendStatus									as [Action],
			Resend_Control.MaxRetryCount							as RetryCount,
			Resend_Control.RetryInterval							as RetryInterval,
			Resend_Control.RetryTimeout								as RetryTimeout,
			Resend_Control.ResendCount 								as ResendIndex,
			Resend_Control.ResendInterval 							as ResendInterval,
			Resend_Control.MaxResendCount 							as MaxResendCount,
			Resend_Control.ResendTimeout 							as ResendTimeout,
			OriginalSend.LastModified								as OriginalSendTime
	
	FROM 		dbo.[bam_ResendTrackingActivity_ActiveInstances] 	as Resend_Control
	LEFT JOIN	dbo.[bam_ResendJournalActivity_CompletedInstances]	as Journal
	ON 		
				Resend_Control.ActivityID 	= Journal.TrackingActivityID
		LEFT JOIN	dbo.[bam_ResendJournalActivity_CompletedInstances]	as OriginalSend
			
			ON 
				(
				Resend_Control.ActivityID 	= OriginalSend.TrackingActivityID
			AND	OriginalSend.ResendStatus	= 'OriginalSendSuccess'
				)
			WHERE
								(
						Resend_Control.ResendCount	= Journal.ResendIndex
					AND	DATEADD(minute, Resend_Control.ResendInterval, Journal.LastModified) < GETUTCDATE()
					AND	(
							Journal.ResendStatus	= 'ResendSuccess'
						OR	Journal.ResendStatus	= 'ResendFailed'
						OR	Journal.ResendStatus	= 'OriginalSendSuccess'
						OR	Journal.ResendStatus	= 'OriginalSendFailed'
						)
					AND	Journal.LastModified > Resend_Control.LastModified
				)
	FOR
			XML AUTO, ELEMENTS

		UPDATE		dbo.[bam_ResendTrackingActivity_Active]
	SET		ResendCount = ResendCount + 1
	WHERE		ActivityID IN
					(
					SELECT Resend_Control.ActivityID
					FROM 		
							dbo.[bam_ResendTrackingActivity_ActiveInstances] 	as Resend_Control
					LEFT JOIN	dbo.[bam_ResendJournalActivity_CompletedInstances]	as Journal
			
						ON 		
							Resend_Control.ActivityID 	= Journal.TrackingActivityID
						WHERE
							(
									Resend_Control.ResendCount	= Journal.ResendIndex
								AND	DATEADD(minute, Resend_Control.ResendInterval, Journal.LastModified) < GETUTCDATE()
								AND	(
										Journal.ResendStatus	= 'ResendSuccess'
									OR	Journal.ResendStatus	= 'ResendFailed'
									OR	Journal.ResendStatus	= 'OriginalSendSuccess'
									OR	Journal.ResendStatus	= 'OriginalSendFailed'
									)
								AND	Journal.LastModified > Resend_Control.LastModified
							)
					)

COMMIT TRANSACTION
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetResendControlMessage] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetResendControlMessage] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetResendControlMessage] TO [BTS_OPERATORS]
    AS [dbo];

