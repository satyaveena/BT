CREATE VIEW dbo.[bam_InterchangeAckActivity_CompletedInstances] AS  SELECT * FROM dbo.[bam_InterchangeAckActivity_Completed] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_InterchangeAckActivity_CompletedInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_InterchangeAckActivity_CompletedInstances] TO [BTS_OPERATORS]
    AS [dbo];

