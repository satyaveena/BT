CREATE VIEW [dbo].[bam_InterchangeStatusActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_InterchangeStatusActivity_Completed] WITH (NOLOCK)
            