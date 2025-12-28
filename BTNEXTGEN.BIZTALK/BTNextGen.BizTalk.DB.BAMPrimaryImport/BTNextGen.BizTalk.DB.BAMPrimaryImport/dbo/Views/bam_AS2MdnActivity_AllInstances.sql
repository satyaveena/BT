CREATE VIEW [dbo].[bam_AS2MdnActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_AS2MdnActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_AS2MdnActivity_CompletedInstances]
            