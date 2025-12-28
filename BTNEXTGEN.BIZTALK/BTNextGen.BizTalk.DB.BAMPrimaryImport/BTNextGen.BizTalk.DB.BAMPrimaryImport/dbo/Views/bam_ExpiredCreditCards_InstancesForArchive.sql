CREATE VIEW [dbo].[bam_ExpiredCreditCards_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_ExpiredCreditCards_Completed] WITH (NOLOCK)
            