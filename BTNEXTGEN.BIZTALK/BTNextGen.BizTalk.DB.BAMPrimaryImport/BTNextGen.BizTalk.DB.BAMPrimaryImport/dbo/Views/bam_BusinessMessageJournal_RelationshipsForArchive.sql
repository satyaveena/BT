CREATE VIEW [dbo].[bam_BusinessMessageJournal_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_BusinessMessageJournal_CompletedRelationships] WITH (NOLOCK)
            