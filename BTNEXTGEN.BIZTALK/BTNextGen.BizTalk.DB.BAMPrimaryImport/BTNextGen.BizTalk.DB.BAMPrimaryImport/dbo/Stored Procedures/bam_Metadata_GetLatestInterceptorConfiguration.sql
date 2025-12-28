-- Get latest interceptor configuration to be used by interceptor
CREATE PROCEDURE [dbo].[bam_Metadata_GetLatestInterceptorConfiguration]
(
	@technologyName NVARCHAR(10),
	@manifest NVARCHAR(440)
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

	DECLARE @searchVersion INT

	-- Find max version
	SELECT @searchVersion = MAX(Version) FROM [dbo].[bam_Metadata_InterceptorConfiguration] WITH (SERIALIZABLE)
	WHERE EventSourceName = @eventSourceName 

	SELECT IC.EventSourceName, IC.ActivityName, IC.Version, ES.EventSourceXml, IC.OnEventXml
	FROM [dbo].[bam_Metadata_InterceptorConfiguration] AS IC JOIN [dbo].[bam_Metadata_EventSource] AS ES
	ON IC.EventSourceName = ES.EventSourceName
	WHERE IC.EventSourceName = @eventSourceName
	AND IC.Version = @searchVersion
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetLatestInterceptorConfiguration] TO [bam_APIDemo_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetLatestInterceptorConfiguration] TO [bam_active_credit_cards_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetLatestInterceptorConfiguration] TO [bam_Cybersource Settlement Feed_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetLatestInterceptorConfiguration] TO [bam_ERPOrders_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetLatestInterceptorConfiguration] TO [bam_ExpiredCreditCards_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetLatestInterceptorConfiguration] TO [bam_FirstDataRecon_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetLatestInterceptorConfiguration] TO [bam_PaymentERPTxn_EventWriter]
    AS [dbo];

