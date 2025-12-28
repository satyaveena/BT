CREATE VIEW dbo.[bam_FunctionalAckActivity_CompletedInstances] AS  SELECT * FROM dbo.[bam_FunctionalAckActivity_Completed] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_FunctionalAckActivity_CompletedInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_FunctionalAckActivity_CompletedInstances] TO [BTS_OPERATORS]
    AS [dbo];

