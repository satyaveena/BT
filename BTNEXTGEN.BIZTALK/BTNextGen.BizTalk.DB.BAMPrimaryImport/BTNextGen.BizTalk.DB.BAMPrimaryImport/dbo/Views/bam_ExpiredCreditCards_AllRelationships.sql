CREATE VIEW dbo.[bam_ExpiredCreditCards_AllRelationships] AS  SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM [bam_ExpiredCreditCards_ActiveRelationships] WITH (NOLOCK) UNION ALL SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM dbo.[bam_ExpiredCreditCards_CompletedRelationships] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_ExpiredCreditCards_AllRelationships] TO [bam_ExpiredCreditCards]
    AS [dbo];

