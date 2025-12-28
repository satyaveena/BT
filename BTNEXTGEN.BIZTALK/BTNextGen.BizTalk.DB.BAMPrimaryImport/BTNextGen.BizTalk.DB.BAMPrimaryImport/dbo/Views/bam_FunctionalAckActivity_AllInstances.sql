CREATE VIEW [dbo].[bam_FunctionalAckActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_FunctionalAckActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_FunctionalAckActivity_CompletedInstances]
            