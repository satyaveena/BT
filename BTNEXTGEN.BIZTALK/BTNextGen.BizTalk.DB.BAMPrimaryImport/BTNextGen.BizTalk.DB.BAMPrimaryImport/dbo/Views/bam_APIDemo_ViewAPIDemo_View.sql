CREATE VIEW dbo.[bam_APIDemo_ViewAPIDemo_View]
        AS
          
          SELECT * FROM dbo.[bam_APIDemo_ViewAPIDemo_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_APIDemo_ViewAPIDemo_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_APIDemo_ViewAPIDemo_View] TO [bam_APIDemo]
    AS [dbo];

