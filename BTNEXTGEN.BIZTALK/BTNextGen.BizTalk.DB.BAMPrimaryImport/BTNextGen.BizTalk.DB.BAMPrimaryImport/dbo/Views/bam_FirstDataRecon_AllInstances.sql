CREATE VIEW [dbo].[bam_FirstDataRecon_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_FirstDataRecon_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_FirstDataRecon_CompletedInstances]
            