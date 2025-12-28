CREATE VIEW [dbo].[bam_Cybersource Settlement Feed_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_Cybersource Settlement Feed_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_Cybersource Settlement Feed_CompletedInstances]
            