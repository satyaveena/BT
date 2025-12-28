CREATE VIEW [dbo].[bam_InterchangeAckActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_InterchangeAckActivity_Completed] WITH (NOLOCK)
            