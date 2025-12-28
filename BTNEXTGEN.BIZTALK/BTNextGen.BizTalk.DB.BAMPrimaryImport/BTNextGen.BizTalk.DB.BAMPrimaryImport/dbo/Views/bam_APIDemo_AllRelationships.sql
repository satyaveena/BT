CREATE VIEW dbo.[bam_APIDemo_AllRelationships] AS  SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM [bam_APIDemo_ActiveRelationships] WITH (NOLOCK) UNION ALL SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'') AS ReferenceData, LongReferenceData FROM dbo.[bam_APIDemo_CompletedRelationships] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_APIDemo_AllRelationships] TO [bam_APIDemo]
    AS [dbo];

