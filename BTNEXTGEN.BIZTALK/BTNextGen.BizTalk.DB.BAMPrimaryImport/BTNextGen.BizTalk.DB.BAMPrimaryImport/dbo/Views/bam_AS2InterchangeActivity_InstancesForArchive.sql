CREATE VIEW [dbo].[bam_AS2InterchangeActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_AS2InterchangeActivity_Completed] WITH (NOLOCK)
            