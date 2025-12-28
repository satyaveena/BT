CREATE PROCEDURE [dbo].[bts_GetBatchStatusRecords]
(
	@destinationPartyName nvarchar(256) = NULL,
	@batchName nvarchar(256) = NULL,
	@batchStatus int = NULL,
	@useStatusEqualsOperator bit = 1,
	@activationStartDateTime datetime = NULL,
	@activationEndDateTime datetime = NULL,
	@ediEncodingType int = NULL,
	@maxRecords int = NULL,
	@debug bit = 0
) AS
BEGIN 
	DECLARE @allSql nvarchar(4000)
	DECLARE @paramlist nvarchar(4000) 

	SET @allSql = 'SELECT baTable.BatchStatus,baTable.BatchName,baTable.DestinationPartyName,
			baTable.ActivationTime, baTable.BatchOccurrenceCount, baTable.EdiEncodingType,
			baTable.BatchType, baTable.TargetedBatchCount, baTable.ScheduledReleaseTime, 
			baTable.BatchElementCount, baTable.RejectedBatchElementCount, 
			baTable.LastBatchAction, baTable.BatchReleaseType, baTable.BatchSize,
			biaTable.SenderPartyName, biaTable.SenderID, biaTable.SenderQ, 
			biaTable.ReceiverPartyName, biaTable.ReceiverID, biaTable.ReceiverQ, biaTable.AgreementName, 
			biaTable.InterchangeControlNo, biaTable.InterchangeDateTime
		FROM bam_BatchingActivity_AllInstances baTable Left Outer Join 
		bam_BatchInterchangeActivity_AllInstances biaTable on
		baTable.BatchCorrelationID = biaTable.BatchCorrelationID
		WHERE 1=1'

	IF @destinationPartyName is NOT NULL 
		SET @allSql = @allSql + ' AND baTable.DestinationPartyName = @destinationPartyName'
	IF @batchName is NOT NULL 
		SET @allSql = @allSql + ' AND baTable.BatchName = @batchName'
	IF @batchStatus is NOT NULL 
	BEGIN
		if @useStatusEqualsOperator = 0
			begin
				if @batchStatus = 3
					begin
						SET @allSql = @allSql + ' AND biaTable.InterchangeControlNo is null'
					end
				else if @batchStatus = 2
					begin
						SET @allSql = @allSql + ' AND ((baTable.BatchStatus <> @batchStatus) or (biaTable.InterchangeControlNo is not null))'
					end
				else
					begin
						SET @allSql = @allSql + ' AND baTable.BatchStatus <> @batchStatus'
					end
			end
		
		else
			begin
				if @batchStatus = 3
					begin
						SET @allSql = @allSql + ' AND biaTable.InterchangeControlNo is not null'
					end
				else if @batchStatus = 2
					begin
						SET @allSql = @allSql + ' AND baTable.BatchStatus = @batchStatus and biaTable.InterchangeControlNo is null'
					end
				else
					begin
						SET @allSql = @allSql + ' AND baTable.BatchStatus = @batchStatus'
					end
			end
	END
	IF @activationStartDateTime is NOT NULL 
		SET @allSql = @allSql + ' AND baTable.ActivationTime >= @activationStartDateTime'
	IF @activationEndDateTime is NOT NULL
		SET @allSql = @allSql + ' AND baTable.ActivationTime <= @activationEndDateTime'
	IF @ediEncodingType is NOT NULL
		SET @allSql = @allSql + ' AND baTable.EdiEncodingType = @ediEncodingType'

	if @debug = 1 
	BEGIN
		PRINT @allSql
		PRINT 'Length of SQL is ' + CAST(LEN(@allSql) as char(10))
	END

	SELECT @paramlist = 
	    	'@destinationPartyName nvarchar(256),
		@batchName nvarchar(256),
		@batchStatus int,
		@activationStartDateTime datetime,
		@activationEndDateTime datetime,
		@ediEncodingType int'

	if @maxRecords is NOT NULL 
		SET ROWCOUNT @maxRecords
	EXEC sp_executesql @allSql, @paramlist,                                   
			@destinationPartyName, @batchName, @batchStatus, @activationStartDateTime, @activationEndDateTime, @ediEncodingType
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetBatchStatusRecords] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetBatchStatusRecords] TO [BTS_OPERATORS]
    AS [dbo];

