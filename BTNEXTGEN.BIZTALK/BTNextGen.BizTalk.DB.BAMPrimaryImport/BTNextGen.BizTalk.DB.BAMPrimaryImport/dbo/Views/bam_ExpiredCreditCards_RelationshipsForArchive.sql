CREATE VIEW [dbo].[bam_ExpiredCreditCards_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_ExpiredCreditCards_CompletedRelationships] WITH (NOLOCK)
            