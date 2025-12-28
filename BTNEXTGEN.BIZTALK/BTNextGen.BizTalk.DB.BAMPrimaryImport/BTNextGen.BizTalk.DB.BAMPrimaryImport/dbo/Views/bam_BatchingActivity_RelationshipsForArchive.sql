CREATE VIEW [dbo].[bam_BatchingActivity_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_BatchingActivity_CompletedRelationships] WITH (NOLOCK)
            