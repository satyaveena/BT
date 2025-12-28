CREATE VIEW [dbo].[bam_active_credit_cards_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_active_credit_cards_CompletedRelationships] WITH (NOLOCK)
            