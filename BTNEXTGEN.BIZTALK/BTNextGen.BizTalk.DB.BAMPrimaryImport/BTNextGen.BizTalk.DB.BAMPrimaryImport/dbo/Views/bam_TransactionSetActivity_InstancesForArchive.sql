CREATE VIEW [dbo].[bam_TransactionSetActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_TransactionSetActivity_Completed] WITH (NOLOCK)
            