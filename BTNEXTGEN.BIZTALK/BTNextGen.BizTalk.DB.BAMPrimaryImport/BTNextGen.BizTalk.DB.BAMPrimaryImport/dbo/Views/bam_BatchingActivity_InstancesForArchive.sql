CREATE VIEW [dbo].[bam_BatchingActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_BatchingActivity_Completed] WITH (NOLOCK)
            