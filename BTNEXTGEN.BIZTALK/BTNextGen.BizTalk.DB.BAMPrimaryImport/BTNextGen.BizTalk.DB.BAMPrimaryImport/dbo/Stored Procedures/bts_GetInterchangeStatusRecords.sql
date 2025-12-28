CREATE PROCEDURE [dbo].[bts_GetInterchangeStatusRecords]
(
	@receiverPartyName nvarchar(256) = NULL,
	@senderPartyName nvarchar(256) = NULL,
	@direction int = NULL,
	@ackStatusCode int = NULL,
	@useStatusEqualsOperator bit = 1,
	@interchangeControlNo nvarchar(14) = NULL,
	@startDate datetime = NULL,
	@endDate datetime = NULL,
	@maxRecords int = NULL,
	@debug bit = 0
) AS
BEGIN 
	DECLARE @x12sql nvarchar(4000) 
	DECLARE @edifactsql nvarchar(4000) 
	DECLARE @allSql nvarchar(4000) 
	DECLARE @paramlist nvarchar(4000) 

				
	SET @x12sql = 'SELECT isaTable.ActivityID, isaTable.InterchangeControlNo, isaTable.ReceiverID, isaTable.SenderID, isaTable.ReceiverQ, 
			isaTable.SenderQ, isaTable.AgreementName, isaTable.ReceiverPartyName, isaTable.SenderPartyName, isaTable.Direction, isaTable.InterchangeDateTime, 
			isaTable.GroupCount, isaTable.PortID,  isaTable.AckStatusCode isaStatusCode, iaaTable.AckStatusCode iaaStatusCode, 
			isaTable.EdiMessageType, isaTable.IsInterchangeAckExpected, isaTable.IsFunctionalAckExpected, isaTable.TsCorrelationId
		FROM bam_InterchangeStatusActivity_CompletedInstances isaTable
		LEFT OUTER JOIN bam_InterchangeAckActivity_CompletedInstances iaaTable
		ON (isaTable.ReceiverID = iaaTable.SenderID AND isaTable.ReceiverQ = iaaTable.SenderQ AND
			isaTable.SenderID = iaaTable.ReceiverID AND isaTable.SenderQ = iaaTable.ReceiverQ AND 
			isaTable.InterchangeControlNo = iaaTable.InterchangeControlNo AND isaTable.Direction = 2 AND iaaTable.Direction <> 2 AND
			isaTable.InterchangeDateTime = iaaTable.InterchangeDateTime AND
				iaaTable.RecordID NOT in ( select RecordID from bts_AllDuplicateInterchangeAckRecordIDs() )
		)
		WHERE isaTable.EdiMessageType=0 '

	IF @receiverPartyName is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.ReceiverPartyName = @receiverPartyName'
	IF @senderPartyName is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.SenderPartyName = @senderPartyName'
	IF @interchangeControlNo is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.InterchangeControlNo = @interchangeControlNo'
	IF @direction is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.Direction = @direction'
	IF @ackStatusCode is NOT NULL 
	BEGIN
		if @useStatusEqualsOperator = 0 
			SET @x12sql = @x12sql + ' AND ((isaTable.AckStatusCode <> @ackStatusCode AND iaaTable.AckStatusCode is NULL)
					 OR iaaTable.AckStatusCode <> @ackStatusCode)'
		else
			SET @x12sql = @x12sql + ' AND ((isaTable.AckStatusCode = @ackStatusCode AND iaaTable.AckStatusCode is NULL)
					 OR iaaTable.AckStatusCode = @ackStatusCode)'
	END
	IF @startDate is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.InterchangeDateTime >= @startDate'
	IF @endDate is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.InterchangeDateTime <= @endDate'

	SET @edifactsql = 'SELECT isaTable.ActivityID, isaTable.InterchangeControlNo, isaTable.ReceiverID, isaTable.SenderID, isaTable.ReceiverQ, 
			isaTable.SenderQ, isaTable.AgreementName, isaTable.ReceiverPartyName, isaTable.SenderPartyName, isaTable.Direction, isaTable.InterchangeDateTime, 
			isaTable.GroupCount, isaTable.PortID,  isaTable.AckStatusCode isaStatusCode, iaaTable.AckStatusCode iaaStatusCode, 
			isaTable.EdiMessageType, isaTable.IsInterchangeAckExpected, isaTable.IsFunctionalAckExpected, isaTable.TsCorrelationId
		FROM bam_InterchangeStatusActivity_CompletedInstances isaTable
		LEFT OUTER JOIN bam_InterchangeAckActivity_CompletedInstances iaaTable
		ON (isaTable.ReceiverID = iaaTable.SenderID AND isaTable.ReceiverQ = iaaTable.SenderQ AND
			isaTable.SenderID = iaaTable.ReceiverID AND isaTable.SenderQ = iaaTable.ReceiverQ AND 
			isaTable.InterchangeControlNo = iaaTable.InterchangeControlNo AND isaTable.Direction = 2 AND iaaTable.Direction <> 2 AND
				iaaTable.RecordID NOT in ( select RecordID from bts_AllDuplicateInterchangeAckRecordIDs() )
		)
		WHERE isaTable.EdiMessageType=1 '

	IF @receiverPartyName is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.ReceiverPartyName = @receiverPartyName'
	IF @senderPartyName is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.SenderPartyName = @senderPartyName'
	IF @interchangeControlNo is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.InterchangeControlNo = @interchangeControlNo'
	IF @direction is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.Direction = @direction'
	IF @ackStatusCode is NOT NULL 
	BEGIN
		if @useStatusEqualsOperator = 0 
			SET @edifactsql = @edifactsql + ' AND ((isaTable.AckStatusCode <> @ackStatusCode AND iaaTable.AckStatusCode is NULL)
				 OR iaaTable.AckStatusCode <> @ackStatusCode)'
		else
			SET @edifactsql = @edifactsql + ' AND ((isaTable.AckStatusCode = @ackStatusCode AND iaaTable.AckStatusCode is NULL)
					 OR iaaTable.AckStatusCode = @ackStatusCode)'
	END
	IF @startDate is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.InterchangeDateTime >= @startDate'
	IF @endDate is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.InterchangeDateTime <= @endDate'

	SET @allSql = @x12sql + '    UNION ALL   ' + @edifactsql
	if @debug = 1 
	BEGIN
		PRINT @allSql
		PRINT 'Length of SQL is ' + CAST(LEN(@allSql) as char(10))
	END

	SELECT @paramlist = 
		'@receiverPartyName nvarchar(256),
		@senderPartyName nvarchar(256),
		@direction int,
		@ackStatusCode int,
		@interchangeControlNo nvarchar(14),
		@startDate datetime,
		@endDate datetime'

	if @maxRecords is NOT NULL 
		SET ROWCOUNT @maxRecords
	EXEC sp_executesql @allSql, @paramlist,                                   
	            @receiverPartyName, @senderPartyName, @direction, @ackStatusCode, @interchangeControlNo, @startDate, @endDate
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetInterchangeStatusRecords] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetInterchangeStatusRecords] TO [BTS_OPERATORS]
    AS [dbo];

