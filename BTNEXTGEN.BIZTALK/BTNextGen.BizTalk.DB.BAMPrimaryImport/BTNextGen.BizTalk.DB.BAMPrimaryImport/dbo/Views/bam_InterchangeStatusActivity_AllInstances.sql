CREATE VIEW [dbo].[bam_InterchangeStatusActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_InterchangeStatusActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_InterchangeStatusActivity_CompletedInstances]
            