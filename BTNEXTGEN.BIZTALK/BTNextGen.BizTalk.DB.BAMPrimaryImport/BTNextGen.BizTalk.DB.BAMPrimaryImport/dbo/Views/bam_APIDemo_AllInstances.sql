CREATE VIEW [dbo].[bam_APIDemo_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_APIDemo_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_APIDemo_CompletedInstances]
            