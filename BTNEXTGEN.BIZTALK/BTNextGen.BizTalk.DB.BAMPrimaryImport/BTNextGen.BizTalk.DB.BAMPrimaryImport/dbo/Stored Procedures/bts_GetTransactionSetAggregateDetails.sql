Create PROCEDURE [dbo].[bts_GetTransactionSetAggregateDetails]
(
	@startDate datetime = NULL,
	@endDate datetime = NULL,
	@ackStatusCode int = NULL,
	@useStatusEqualsOperator bit = 1,
	@senderPartyName nvarchar(256) = NULL,
	@receiverPartyName nvarchar(256) = NULL,
	@direction int = NULL,
	@transactionSetId nvarchar(6) = NULL,
	@maxRecords int = NULL,
	@debug bit = 0
) AS
BEGIN 
	DECLARE @tssql nvarchar(4000) 
	DECLARE @paramlist nvarchar(4000) 
				
	SET @tssql = 'SELECT tsaTable.TransactionSetId, Count(*) As TransactionSetCount, 
			tsaTable.ReceiverPartyName, tsaTable.SenderPartyName, tsaTable.AgreementName, tsaTable.Direction,
			min(tsaTable.InterchangeDateTime) as MinInterchangeDateTime,
			max(tsaTable.InterchangeDateTime) as MaxInterchangeDateTime
			FROM bam_TransactionSetActivity_CompletedInstances tsaTable
			Where 1=1'

	IF @startDate is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.InterchangeDateTime >= @startDate'
	IF @endDate is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.InterchangeDateTime <= @endDate'
	IF @ackStatusCode is NOT NULL 
	BEGIN
		if @useStatusEqualsOperator = 0 
			SET @tssql = @tssql + ' AND tsaTable.AckStatusCode <> @ackStatusCode'
		else
			SET @tssql = @tssql + ' AND tsaTable.AckStatusCode = @ackStatusCode'
	END
	IF @receiverPartyName is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.ReceiverPartyName = @receiverPartyName'
	IF @senderPartyName is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.SenderPartyName = @senderPartyName'
	IF @direction is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.Direction = @direction'
	IF @transactionSetId is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.TransactionSetId = @transactionSetId'

	SET @tssql = @tssql + ' GROUP BY tsaTable.TransactionSetId, tsaTable.ReceiverPartyName, tsaTable.SenderPartyName, tsaTable.AgreementName, tsaTable.Direction'
	if @debug = 1 
	BEGIN
		PRINT @tssql
		PRINT 'Length of SQL is ' + CAST(LEN(@tssql) as char(10))
	END

	SELECT @paramlist = 
		'@startDate datetime,
		@endDate datetime,
		@ackStatusCode int,
		@senderPartyName nvarchar(256),
		@receiverPartyName nvarchar(256),
		@direction int,
		@transactionSetId nvarchar(6)'

	if @maxRecords is NOT NULL 
		SET ROWCOUNT @maxRecords
	EXEC sp_executesql @tssql, @paramlist,
				@startDate, @endDate, @ackStatusCode, @senderPartyName,
				@receiverPartyName, @direction, @transactionSetId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetTransactionSetAggregateDetails] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetTransactionSetAggregateDetails] TO [BTS_OPERATORS]
    AS [dbo];

