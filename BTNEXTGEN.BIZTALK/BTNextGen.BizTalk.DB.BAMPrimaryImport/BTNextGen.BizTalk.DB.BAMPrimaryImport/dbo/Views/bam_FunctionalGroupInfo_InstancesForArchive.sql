CREATE VIEW [dbo].[bam_FunctionalGroupInfo_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_FunctionalGroupInfo_Completed] WITH (NOLOCK)
            