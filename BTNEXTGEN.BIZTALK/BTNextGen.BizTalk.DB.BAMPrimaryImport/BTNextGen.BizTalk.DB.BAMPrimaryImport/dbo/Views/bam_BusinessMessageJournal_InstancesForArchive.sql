CREATE VIEW [dbo].[bam_BusinessMessageJournal_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_BusinessMessageJournal_Completed] WITH (NOLOCK)
            