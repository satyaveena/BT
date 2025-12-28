CREATE VIEW dbo.[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_View]
        AS
          
          SELECT * FROM dbo.[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_View] TO [bam_CyberSourceFeedV]
    AS [dbo];

