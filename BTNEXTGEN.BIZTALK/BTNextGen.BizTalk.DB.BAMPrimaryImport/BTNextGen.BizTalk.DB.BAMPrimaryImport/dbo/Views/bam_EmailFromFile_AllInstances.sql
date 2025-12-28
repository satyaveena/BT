CREATE VIEW [dbo].[bam_EmailFromFile_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_EmailFromFile_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_EmailFromFile_CompletedInstances]
            