CREATE VIEW [dbo].[bam_FirstDataRecon_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_FirstDataRecon_Completed] WITH (NOLOCK)
            