CREATE VIEW dbo.[bam_FirstDataRecon_AllRelationships] AS  SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM [bam_FirstDataRecon_ActiveRelationships] WITH (NOLOCK) UNION ALL SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM dbo.[bam_FirstDataRecon_CompletedRelationships] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_FirstDataRecon_AllRelationships] TO [bam_FirstDataReconView]
    AS [dbo];

