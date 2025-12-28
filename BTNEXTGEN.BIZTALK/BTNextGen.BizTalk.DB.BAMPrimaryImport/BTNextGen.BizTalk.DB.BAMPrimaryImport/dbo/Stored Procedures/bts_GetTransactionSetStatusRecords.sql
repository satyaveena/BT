Create PROCEDURE [dbo].[bts_GetTransactionSetStatusRecords]
(
	@transactionSetStartDate datetime = NULL,
	@transactionSetEndDate datetime = NULL,
	@ackStatusCode int = NULL,
	@useStatusEqualsOperator bit = 1,
	@senderPartyName nvarchar(256) = NULL,
	@receiverPartyName nvarchar(256) = NULL,
	@direction int = NULL,
	@transactionSetId nvarchar(6) = NULL,
	@useTsIdEqualsOperator bit = 1,
	@interchangeControlNo nvarchar(14) = NULL,
	@TsCorrelationId nvarchar(32) = null,
	@maxRecords int = NULL,
	@debug bit = 0
) AS
BEGIN 
	DECLARE @tssql nvarchar(4000) 
	DECLARE @paramlist nvarchar(4000) 
				
	SET @tssql = 'SELECT tsaTable.TransactionSetId, tsaTable.DocType, tsaTable.SenderID, tsaTable.SenderQ, tsaTable.ApplicationSender, tsaTable.ReceiverID,
			tsaTable.ReceiverQ, tsaTable.AgreementName, tsaTable.ApplicationReceiver, tsaTable.ReceiverPartyName, tsaTable.SenderPartyName, tsaTable.Direction, 
			tsaTable.InterchangeControlNo, tsaTable.InterchangeDateTime, tsaTable.GroupControlNo, tsaTable.TransactionSetControlNo, tsaTable.AckStatusCode, tsaTable.GroupDateTime, 
			tsaTable.ProcessingDateTime, tsaTable.MessageContentKey
			FROM bam_TransactionSetActivity_CompletedInstances tsaTable
			Where 1=1'

	IF @transactionSetStartDate is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.InterchangeDateTime >= @transactionSetStartDate'
	IF @transactionSetEndDate is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.InterchangeDateTime <= @transactionSetEndDate'
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
	BEGIN
		if @useTsIdEqualsOperator = 0 
			SET @tssql = @tssql + ' AND tsaTable.TransactionSetId <> @transactionSetId'
		else
			SET @tssql = @tssql + ' AND tsaTable.TransactionSetId = @transactionSetId'
	END
	IF @interchangeControlNo is NOT NULL 
		SET @tssql = @tssql + ' AND tsaTable.InterchangeControlNo = @interchangeControlNo'

	IF @TsCorrelationId is NOT NULL
		SET @tssql = @tssql + ' AND tsaTable.TsCorrelationId = @TsCorrelationId'

	if @debug = 1 
	BEGIN
		PRINT @tssql
		PRINT 'Length of SQL is ' + CAST(LEN(@tssql) as char(10))
	END

	SELECT @paramlist = 
		'@transactionSetStartDate datetime,
		@transactionSetEndDate datetime,
		@ackStatusCode int,
		@senderPartyName nvarchar(256),
		@receiverPartyName nvarchar(256),
		@direction int,
		@transactionSetId nvarchar(6),
		@interchangeControlNo nvarchar(14),
		@TsCorrelationId nvarchar(32)'

	if @maxRecords is NOT NULL 
		SET ROWCOUNT @maxRecords
	EXEC sp_executesql @tssql, @paramlist,
				@transactionSetStartDate, @transactionSetEndDate, @ackStatusCode, @senderPartyName,
				@receiverPartyName, @direction, @transactionSetId, @interchangeControlNo, @TsCorrelationId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetTransactionSetStatusRecords] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetTransactionSetStatusRecords] TO [BTS_OPERATORS]
    AS [dbo];

