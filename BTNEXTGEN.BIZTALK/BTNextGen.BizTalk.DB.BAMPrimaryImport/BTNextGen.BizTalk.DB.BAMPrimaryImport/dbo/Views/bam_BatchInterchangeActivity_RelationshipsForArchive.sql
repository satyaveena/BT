CREATE VIEW [dbo].[bam_BatchInterchangeActivity_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_BatchInterchangeActivity_CompletedRelationships] WITH (NOLOCK)
            