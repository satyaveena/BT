CREATE VIEW [dbo].[bam_EmailFromFile_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_EmailFromFile_Completed] WITH (NOLOCK)
            