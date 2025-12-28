CREATE VIEW dbo.[bam_EmailProcess_ViewEmailFromFile_View]
        AS
          
          SELECT * FROM dbo.[bam_EmailProcess_ViewEmailFromFile_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_EmailProcess_ViewEmailFromFile_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_EmailProcess_ViewEmailFromFile_View] TO [bam_EmailProcess]
    AS [dbo];

