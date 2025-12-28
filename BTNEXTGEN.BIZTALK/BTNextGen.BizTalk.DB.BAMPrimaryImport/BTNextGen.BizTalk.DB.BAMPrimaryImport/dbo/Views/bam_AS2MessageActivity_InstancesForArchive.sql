CREATE VIEW [dbo].[bam_AS2MessageActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_AS2MessageActivity_Completed] WITH (NOLOCK)
            