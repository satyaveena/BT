CREATE VIEW dbo.[bam_ERPOrders_ViewERPOrders_View]
        AS
          
          SELECT * FROM dbo.[bam_ERPOrders_ViewERPOrders_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_ERPOrders_ViewERPOrders_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_ERPOrders_ViewERPOrders_View] TO [bam_ERPOrders]
    AS [dbo];

