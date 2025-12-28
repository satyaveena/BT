CREATE VIEW [dbo].[bam_PaymentERPTxn_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_PaymentERPTxn_CompletedRelationships] WITH (NOLOCK)
            