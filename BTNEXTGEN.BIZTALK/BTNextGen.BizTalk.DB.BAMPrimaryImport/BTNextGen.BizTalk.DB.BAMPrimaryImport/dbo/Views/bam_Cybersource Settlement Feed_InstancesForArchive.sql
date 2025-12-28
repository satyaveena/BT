CREATE VIEW [dbo].[bam_Cybersource Settlement Feed_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_Cybersource Settlement Feed_Completed] WITH (NOLOCK)
            