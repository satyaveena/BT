CREATE VIEW [dbo].[bam_ERPOrders_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_ERPOrders_CompletedRelationships] WITH (NOLOCK)
            