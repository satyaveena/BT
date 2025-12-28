CREATE PROCEDURE [dbo].[bts_GetInterchangeAggregateDetails]
(
	@startDate datetime = NULL,
	@endDate datetime = NULL,
	@ackStatusCode int = NULL,
	@useStatusEqualsOperator bit = 1,
	@receiverPartyName  nvarchar(256) = NULL,
	@senderPartyName nvarchar(256) = NULL,
	@direction int = NULL,
	@maxRecords int = NULL,
	@debug bit = 0
) AS
BEGIN 
	DECLARE @x12sql nvarchar(4000) 
	DECLARE @edifactsql nvarchar(4000) 
	DECLARE @allSql nvarchar(4000) 
	DECLARE @paramlist nvarchar(4000) 

				
	SET @x12sql = 'SELECT Count(*) as InterchangeCount, isaTable.ReceiverPartyName, isaTable.SenderPartyName, isaTable.AgreementName, isaTable.Direction, 
				min(isaTable.InterchangeDateTime) as MinInterchangeDateTime, max(isaTable.InterchangeDateTime) as MaxInterchangeDateTime, 
				isaTable.EdiMessageType
			FROM bam_InterchangeStatusActivity_CompletedInstances isaTable
			LEFT OUTER JOIN bam_InterchangeAckActivity_CompletedInstances iaaTable
				ON (isaTable.ReceiverID = iaaTable.SenderID AND isaTable.ReceiverQ = iaaTable.SenderQ AND
					isaTable.SenderID = iaaTable.ReceiverID AND isaTable.SenderQ = iaaTable.ReceiverQ AND 
					isaTable.InterchangeControlNo = iaaTable.InterchangeControlNo AND isaTable.Direction = 2 AND 
					isaTable.InterchangeDateTime = iaaTable.InterchangeDateTime AND
					iaaTable.RecordID NOT in (select RecordID from bts_AllDuplicateInterchangeAckRecordIDs()))
			WHERE isaTable.EdiMessageType=0 '

	IF @startDate is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.InterchangeDateTime >= @startDate'
	IF @endDate is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.InterchangeDateTime <= @endDate'
	IF @receiverPartyName is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.ReceiverPartyName = @receiverPartyName'
	IF @senderPartyName is NOT NULL 
		SET @x12sql = @x12sql + ' AND isaTable.SenderPartyName = @senderPartyName'
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
	SET @x12sql = @x12sql + ' GROUP BY isaTable.SenderPartyName, isaTable.ReceiverPartyName, isaTable.Direction, isaTable.EdiMessageType, isaTable.AgreementName'

	SET @edifactsql = 'SELECT Count(*) as InterchangeCount, isaTable.ReceiverPartyName, isaTable.SenderPartyName, isaTable.AgreementName, isaTable.Direction, 
				min(isaTable.InterchangeDateTime) as MinInterchangeDateTime, max(isaTable.InterchangeDateTime) as MaxInterchangeDateTime,
				isaTable.EdiMessageType
			FROM bam_InterchangeStatusActivity_CompletedInstances isaTable
			LEFT OUTER JOIN bam_InterchangeAckActivity_CompletedInstances iaaTable
				ON (isaTable.ReceiverID = iaaTable.SenderID AND isaTable.ReceiverQ = iaaTable.SenderQ AND
					isaTable.SenderID = iaaTable.ReceiverID AND isaTable.SenderQ = iaaTable.ReceiverQ AND 
					isaTable.InterchangeControlNo = iaaTable.InterchangeControlNo AND isaTable.Direction = 2 AND 
					iaaTable.RecordID NOT in ( select RecordID from bts_AllDuplicateInterchangeAckRecordIDs()))
			WHERE isaTable.EdiMessageType=1 '

	IF @startDate is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.InterchangeDateTime >= @startDate'
	IF @endDate is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.InterchangeDateTime <= @endDate'	IF @receiverPartyName is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.ReceiverPartyName = @receiverPartyName'
	IF @senderPartyName is NOT NULL 
		SET @edifactsql = @edifactsql + ' AND isaTable.SenderPartyName = @senderPartyName'
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

	SET @edifactsql = @edifactsql + ' GROUP BY isaTable.SenderPartyName, isaTable.ReceiverPartyName, isaTable.Direction, isaTable.EdiMessageType, isaTable.AgreementName'


	SET @allSql = @x12sql + '    UNION ALL  ' + @edifactsql
	if @debug = 1 
	BEGIN
		PRINT @allSql
		PRINT 'Length of SQL is ' + CAST(LEN(@allSql) as char(10))
	END

	SELECT @paramlist = 
		'@startDate datetime,
		 @endDate datetime,
		 @ackStatusCode int,
		 @receiverPartyName nvarchar(256),
		 @senderPartyName nvarchar(256),
		 @direction int'
		 

	if @maxRecords is NOT NULL 
		SET ROWCOUNT @maxRecords
	EXEC sp_executesql @allSql, @paramlist,                                   
	            @startDate, @endDate, @ackStatusCode, @receiverPartyName, @senderPartyName, @direction
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetInterchangeAggregateDetails] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetInterchangeAggregateDetails] TO [BTS_OPERATORS]
    AS [dbo];

