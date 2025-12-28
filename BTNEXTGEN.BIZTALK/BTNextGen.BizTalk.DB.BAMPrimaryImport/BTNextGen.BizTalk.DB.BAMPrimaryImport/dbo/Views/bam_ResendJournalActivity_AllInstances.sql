CREATE VIEW [dbo].[bam_ResendJournalActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_ResendJournalActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_ResendJournalActivity_CompletedInstances]
            