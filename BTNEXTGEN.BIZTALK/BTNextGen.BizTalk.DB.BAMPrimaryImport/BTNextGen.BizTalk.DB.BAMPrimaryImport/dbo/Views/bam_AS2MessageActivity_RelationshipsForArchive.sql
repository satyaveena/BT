CREATE VIEW [dbo].[bam_AS2MessageActivity_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_AS2MessageActivity_CompletedRelationships] WITH (NOLOCK)
            