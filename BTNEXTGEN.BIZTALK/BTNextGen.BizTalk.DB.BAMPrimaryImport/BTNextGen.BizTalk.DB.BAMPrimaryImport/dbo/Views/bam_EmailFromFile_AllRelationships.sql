CREATE VIEW dbo.[bam_EmailFromFile_AllRelationships] AS  SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM [bam_EmailFromFile_ActiveRelationships] WITH (NOLOCK) UNION ALL SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM dbo.[bam_EmailFromFile_CompletedRelationships] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_EmailFromFile_AllRelationships] TO [bam_EmailTotals]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_EmailFromFile_AllRelationships] TO [bam_EmailProcess]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_EmailFromFile_AllRelationships] TO [bam_EmailTracking]
    AS [dbo];

