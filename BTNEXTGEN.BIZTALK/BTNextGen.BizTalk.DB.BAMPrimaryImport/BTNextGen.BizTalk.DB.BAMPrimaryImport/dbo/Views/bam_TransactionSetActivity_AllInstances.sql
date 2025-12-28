CREATE VIEW [dbo].[bam_TransactionSetActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_TransactionSetActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_TransactionSetActivity_CompletedInstances]
            