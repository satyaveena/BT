CREATE VIEW [dbo].[bam_active_credit_cards_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_active_credit_cards_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_active_credit_cards_CompletedInstances]
            