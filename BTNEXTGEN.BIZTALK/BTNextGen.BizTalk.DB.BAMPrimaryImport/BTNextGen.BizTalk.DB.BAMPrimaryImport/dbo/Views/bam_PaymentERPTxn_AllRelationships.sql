CREATE VIEW dbo.[bam_PaymentERPTxn_AllRelationships] AS  SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM [bam_PaymentERPTxn_ActiveRelationships] WITH (NOLOCK) UNION ALL SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM dbo.[bam_PaymentERPTxn_CompletedRelationships] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_PaymentERPTxn_AllRelationships] TO [bam_PaymentERPTxn]
    AS [dbo];

