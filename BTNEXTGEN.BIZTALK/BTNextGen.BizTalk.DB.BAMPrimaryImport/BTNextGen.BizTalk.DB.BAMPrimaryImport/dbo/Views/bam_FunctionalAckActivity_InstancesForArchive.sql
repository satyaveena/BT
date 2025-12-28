CREATE VIEW [dbo].[bam_FunctionalAckActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_FunctionalAckActivity_Completed] WITH (NOLOCK)
            