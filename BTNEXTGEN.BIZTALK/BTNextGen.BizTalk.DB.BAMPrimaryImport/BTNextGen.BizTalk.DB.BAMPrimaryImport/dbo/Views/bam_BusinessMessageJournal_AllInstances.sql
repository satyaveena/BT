CREATE VIEW [dbo].[bam_BusinessMessageJournal_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_BusinessMessageJournal_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_BusinessMessageJournal_CompletedInstances]
            