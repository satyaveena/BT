CREATE VIEW [dbo].[bam_PaymentERPTxn_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_PaymentERPTxn_Completed] WITH (NOLOCK)
            