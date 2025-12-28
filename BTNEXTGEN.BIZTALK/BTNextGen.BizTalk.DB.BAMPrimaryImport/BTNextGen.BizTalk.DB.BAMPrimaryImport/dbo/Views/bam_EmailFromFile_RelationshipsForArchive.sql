CREATE VIEW [dbo].[bam_EmailFromFile_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_EmailFromFile_CompletedRelationships] WITH (NOLOCK)
            