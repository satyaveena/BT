CREATE VIEW [dbo].[bam_AS2InterchangeActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_AS2InterchangeActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_AS2InterchangeActivity_CompletedInstances]
            