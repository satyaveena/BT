
CREATE PROCEDURE [dbo].[bam_Metadata_GetRTAConnectionString]
(
    @cubeName   sysname,
    @rtaName    sysname
)
AS
    DECLARE @serverName sysname
    DECLARE @databaseName sysname
    
    SELECT @serverName = PropertyValue
    FROM bam_Metadata_Properties
    WHERE Scope = N'PrimaryImportDatabase' AND PropertyName = N'ServerName'
    
    SELECT @databaseName = PropertyValue
    FROM bam_Metadata_Properties
    WHERE Scope = N'PrimaryImportDatabase' AND PropertyName = N'DatabaseName'

    SELECT REPLACE(REPLACE(ConnectionString, N'{0}', @serverName), N'{1}', @databaseName) AS ConnectionString
    FROM [dbo].[bam_Metadata_RealTimeAggregations]
    WHERE CubeName = @cubeName AND RtaName = @rtaName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_EmailTotals]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_EmailProcess]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_EmailTracking]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_APIDemo]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_active_CCards_view]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_CyberSourceFeedV]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_ERPOrders]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_ExpiredCreditCards]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_FirstDataReconView]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAConnectionString] TO [bam_PaymentERPTxn]
    AS [dbo];

