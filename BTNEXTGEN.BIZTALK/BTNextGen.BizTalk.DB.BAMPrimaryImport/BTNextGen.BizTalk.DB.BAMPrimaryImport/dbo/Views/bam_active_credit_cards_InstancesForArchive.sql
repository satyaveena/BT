CREATE VIEW [dbo].[bam_active_credit_cards_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_active_credit_cards_Completed] WITH (NOLOCK)
            