CREATE VIEW [dbo].[bam_ResendTrackingActivity_RelationshipsForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_ResendTrackingActivity_CompletedRelationships] WITH (NOLOCK)
            