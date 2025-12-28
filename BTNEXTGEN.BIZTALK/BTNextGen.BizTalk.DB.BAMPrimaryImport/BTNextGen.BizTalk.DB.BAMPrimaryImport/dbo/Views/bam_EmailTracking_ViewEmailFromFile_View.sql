CREATE VIEW dbo.[bam_EmailTracking_ViewEmailFromFile_View]
        AS
          
          SELECT * FROM dbo.[bam_EmailTracking_ViewEmailFromFile_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_EmailTracking_ViewEmailFromFile_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_EmailTracking_ViewEmailFromFile_View] TO [bam_EmailTracking]
    AS [dbo];

