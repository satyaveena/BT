-- Get specific version of interceptor configuration to be used by interceptor
CREATE PROCEDURE [dbo].[bam_Metadata_GetSpecificInterceptorConfiguration]
(
	@technologyName NVARCHAR(10),
	@manifest NVARCHAR(440),
	@version INT
)
AS
	DECLARE @eventSourceName NVARCHAR(128)
	SET @eventSourceName = NULL

	-- Find event source name
	SELECT @eventSourceName = EventSourceName FROM [dbo].[bam_Metadata_EventSource] WITH (SERIALIZABLE)
	WHERE TechnologyName = @technologyName AND Manifest = @manifest 

	IF @eventSourceName IS NULL
	BEGIN
		RETURN
	END

	SELECT IC.EventSourceName, IC.ActivityName, IC.Version, ES.EventSourceXml, IC.OnEventXml
	FROM [dbo].[bam_Metadata_InterceptorConfiguration] AS IC WITH (SERIALIZABLE) JOIN [dbo].[bam_Metadata_EventSource] AS ES
	ON IC.EventSourceName = ES.EventSourceName
	WHERE IC.EventSourceName = @eventSourceName
	AND IC.Version = @version
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetSpecificInterceptorConfiguration] TO [bam_APIDemo_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetSpecificInterceptorConfiguration] TO [bam_active_credit_cards_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetSpecificInterceptorConfiguration] TO [bam_Cybersource Settlement Feed_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetSpecificInterceptorConfiguration] TO [bam_ERPOrders_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetSpecificInterceptorConfiguration] TO [bam_ExpiredCreditCards_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetSpecificInterceptorConfiguration] TO [bam_FirstDataRecon_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetSpecificInterceptorConfiguration] TO [bam_PaymentERPTxn_EventWriter]
    AS [dbo];

