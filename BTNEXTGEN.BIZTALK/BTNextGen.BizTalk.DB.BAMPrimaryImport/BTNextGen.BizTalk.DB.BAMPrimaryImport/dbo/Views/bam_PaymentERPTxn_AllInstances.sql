CREATE VIEW [dbo].[bam_PaymentERPTxn_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_PaymentERPTxn_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_PaymentERPTxn_CompletedInstances]
            