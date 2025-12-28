CREATE VIEW dbo.[bam_InterchangeStatusActivity_CompletedInstances] AS  SELECT * FROM dbo.[bam_InterchangeStatusActivity_Completed] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_InterchangeStatusActivity_CompletedInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_InterchangeStatusActivity_CompletedInstances] TO [BTS_OPERATORS]
    AS [dbo];

