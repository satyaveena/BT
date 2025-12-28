CREATE VIEW [dbo].[bam_ResendJournalActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_ResendJournalActivity_Completed] WITH (NOLOCK)
            