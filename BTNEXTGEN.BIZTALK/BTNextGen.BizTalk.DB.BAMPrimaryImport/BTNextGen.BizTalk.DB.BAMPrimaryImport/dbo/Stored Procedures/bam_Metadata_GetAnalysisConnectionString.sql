
CREATE PROCEDURE [dbo].[bam_Metadata_GetAnalysisConnectionString]
AS
    DECLARE @@analysisServer NVARCHAR(128), @@analysisDatabase NVARCHAR(128)
    SELECT @@analysisServer = PropertyValue
    FROM [dbo].[bam_Metadata_Properties]
    WHERE Scope = N'AnalysisDatabase' AND PropertyName = N'ServerName'
    
    SELECT @@analysisDatabase = PropertyValue
    FROM [dbo].[bam_Metadata_Properties]
    WHERE Scope = N'AnalysisDatabase' AND PropertyName = N'DatabaseName'

    SELECT REPLACE(REPLACE(N'OLEDB; Provider=MSOLAP; Data Source={0};Initial Catalog={1}', N'{0}', @@analysisServer), N'{1}', @@analysisDatabase) AS ConnectionString

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_EmailTotals]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_EmailProcess]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_EmailTracking]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_APIDemo]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_active_CCards_view]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_CyberSourceFeedV]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_ERPOrders]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_ExpiredCreditCards]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_FirstDataReconView]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnalysisConnectionString] TO [bam_PaymentERPTxn]
    AS [dbo];

