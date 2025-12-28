-- Get interceptor configuration to be used by interceptor
CREATE PROCEDURE [dbo].[bam_Metadata_TryGetUpdatedInterceptorConfiguration]
(
	@technologyName NVARCHAR(10),
	@manifest NVARCHAR(440),
	@version INT,
	@isRemoved INT OUTPUT
)
AS	
	DECLARE @eventSourceName NVARCHAR(128)
	SET @eventSourceName = NULL

	-- Default
	SET @isRemoved = 0

	-- Find event source name
	SELECT @eventSourceName = EventSourceName FROM [dbo].[bam_Metadata_EventSource] WITH (SERIALIZABLE)
	WHERE TechnologyName = @technologyName AND Manifest = @manifest

	IF @eventSourceName IS NULL
	BEGIN
		SET @isRemoved = 1
		RETURN
	END

	DECLARE @searchVersion INT

	-- Find max version
	SELECT @searchVersion = MAX(Version) FROM [dbo].[bam_Metadata_InterceptorConfiguration] WITH (SERIALIZABLE)		
	WHERE EventSourceName = @eventSourceName 
			
	-- Check that such an IC exists
	IF @searchVersion IS NULL
	BEGIN
		SET @isRemoved = 1
		RETURN
	END	

	-- Check that there is updated IC to return		
	IF @searchVersion <= @version 
	BEGIN
		-- Return nothing
		RETURN
	END 

	SELECT IC.EventSourceName, IC.ActivityName, IC.Version, ES.EventSourceXml, IC.OnEventXml
	FROM [dbo].[bam_Metadata_InterceptorConfiguration] AS IC JOIN [dbo].[bam_Metadata_EventSource] AS ES
	ON IC.EventSourceName = ES.EventSourceName
	WHERE IC.EventSourceName = @eventSourceName
	AND IC.Version = @searchVersion
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_TryGetUpdatedInterceptorConfiguration] TO [bam_APIDemo_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_TryGetUpdatedInterceptorConfiguration] TO [bam_active_credit_cards_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_TryGetUpdatedInterceptorConfiguration] TO [bam_Cybersource Settlement Feed_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_TryGetUpdatedInterceptorConfiguration] TO [bam_ERPOrders_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_TryGetUpdatedInterceptorConfiguration] TO [bam_ExpiredCreditCards_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_TryGetUpdatedInterceptorConfiguration] TO [bam_FirstDataRecon_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_TryGetUpdatedInterceptorConfiguration] TO [bam_PaymentERPTxn_EventWriter]
    AS [dbo];

