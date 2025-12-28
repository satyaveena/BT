CREATE VIEW [dbo].[bam_FunctionalGroupInfo_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_FunctionalGroupInfo_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_FunctionalGroupInfo_CompletedInstances]
            