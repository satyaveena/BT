CREATE VIEW [dbo].[bam_ERPOrders_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_ERPOrders_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_ERPOrders_CompletedInstances]
            