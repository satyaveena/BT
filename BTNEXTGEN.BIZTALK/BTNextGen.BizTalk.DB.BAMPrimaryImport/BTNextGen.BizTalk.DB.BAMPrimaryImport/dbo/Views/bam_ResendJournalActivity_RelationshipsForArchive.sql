CREATE VIEW [dbo].[bam_ResendJournalActivity_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_ResendJournalActivity_CompletedRelationships] WITH (NOLOCK)
            