CREATE VIEW [dbo].[bam_FunctionalAckActivity_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_FunctionalAckActivity_CompletedRelationships] WITH (NOLOCK)
            