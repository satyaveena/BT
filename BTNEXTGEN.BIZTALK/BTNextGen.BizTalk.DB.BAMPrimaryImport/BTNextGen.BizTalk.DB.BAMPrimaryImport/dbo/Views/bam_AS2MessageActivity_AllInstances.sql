CREATE VIEW [dbo].[bam_AS2MessageActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_AS2MessageActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_AS2MessageActivity_CompletedInstances]
            