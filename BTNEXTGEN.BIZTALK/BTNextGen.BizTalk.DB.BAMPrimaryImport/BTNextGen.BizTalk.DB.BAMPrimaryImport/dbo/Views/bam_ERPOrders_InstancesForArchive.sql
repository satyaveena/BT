CREATE VIEW [dbo].[bam_ERPOrders_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_ERPOrders_Completed] WITH (NOLOCK)
            