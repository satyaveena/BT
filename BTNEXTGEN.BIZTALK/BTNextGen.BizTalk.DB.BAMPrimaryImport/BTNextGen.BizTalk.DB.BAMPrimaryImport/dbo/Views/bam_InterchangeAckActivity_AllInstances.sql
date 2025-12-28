CREATE VIEW [dbo].[bam_InterchangeAckActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_InterchangeAckActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_InterchangeAckActivity_CompletedInstances]
            