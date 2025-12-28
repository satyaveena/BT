CREATE VIEW dbo.[bam_active_credit_cards_AllRelationships] AS  SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM [bam_active_credit_cards_ActiveRelationships] WITH (NOLOCK) UNION ALL SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM dbo.[bam_active_credit_cards_CompletedRelationships] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_active_credit_cards_AllRelationships] TO [bam_active_CCards_view]
    AS [dbo];

