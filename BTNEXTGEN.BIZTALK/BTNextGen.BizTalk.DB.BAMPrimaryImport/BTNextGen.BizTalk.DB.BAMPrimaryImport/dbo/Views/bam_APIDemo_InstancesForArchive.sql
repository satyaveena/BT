CREATE VIEW [dbo].[bam_APIDemo_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_APIDemo_Completed] WITH (NOLOCK)
            