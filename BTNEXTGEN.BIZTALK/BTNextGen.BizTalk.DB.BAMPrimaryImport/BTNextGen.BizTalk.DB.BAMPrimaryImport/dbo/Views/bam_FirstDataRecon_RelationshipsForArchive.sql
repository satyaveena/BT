CREATE VIEW [dbo].[bam_FirstDataRecon_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_FirstDataRecon_CompletedRelationships] WITH (NOLOCK)
            