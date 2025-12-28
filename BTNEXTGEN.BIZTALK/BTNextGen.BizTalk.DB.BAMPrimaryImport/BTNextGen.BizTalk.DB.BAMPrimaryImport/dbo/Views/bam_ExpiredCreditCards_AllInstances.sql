CREATE VIEW [dbo].[bam_ExpiredCreditCards_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_ExpiredCreditCards_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_ExpiredCreditCards_CompletedInstances]
            