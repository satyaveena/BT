CREATE VIEW [dbo].[bam_BatchInterchangeActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_BatchInterchangeActivity_Completed] WITH (NOLOCK)
            