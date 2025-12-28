CREATE VIEW dbo.[bam_EmailTotals_ViewEmailFromFile_View]
        AS
          
          SELECT * FROM dbo.[bam_EmailTotals_ViewEmailFromFile_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_EmailTotals_ViewEmailFromFile_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_EmailTotals_ViewEmailFromFile_View] TO [bam_EmailTotals]
    AS [dbo];

