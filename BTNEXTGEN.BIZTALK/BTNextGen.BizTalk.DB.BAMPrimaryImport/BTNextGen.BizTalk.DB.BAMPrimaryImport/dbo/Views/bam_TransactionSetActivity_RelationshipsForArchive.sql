CREATE VIEW [dbo].[bam_TransactionSetActivity_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_TransactionSetActivity_CompletedRelationships] WITH (NOLOCK)
            