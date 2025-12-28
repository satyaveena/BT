CREATE VIEW dbo.[bam_AS2MdnActivity_CompletedInstances] AS  SELECT * FROM dbo.[bam_AS2MdnActivity_Completed] WITH (NOLOCK)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_AS2MdnActivity_CompletedInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_AS2MdnActivity_CompletedInstances] TO [BTS_OPERATORS]
    AS [dbo];

