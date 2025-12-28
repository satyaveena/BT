CREATE VIEW [dbo].[bam_InterchangeAckActivity_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_InterchangeAckActivity_CompletedRelationships] WITH (NOLOCK)
            