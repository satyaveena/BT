CREATE VIEW [dbo].[bam_AS2MdnActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_AS2MdnActivity_Completed] WITH (NOLOCK)
            