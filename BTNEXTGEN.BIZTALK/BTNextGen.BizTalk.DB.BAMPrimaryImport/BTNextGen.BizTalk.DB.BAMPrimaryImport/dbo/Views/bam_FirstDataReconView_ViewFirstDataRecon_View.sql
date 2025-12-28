CREATE VIEW dbo.[bam_FirstDataReconView_ViewFirstDataRecon_View]
        AS
          
          SELECT * FROM dbo.[bam_FirstDataReconView_ViewFirstDataRecon_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_FirstDataReconView_ViewFirstDataRecon_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_FirstDataReconView_ViewFirstDataRecon_View] TO [bam_FirstDataReconView]
    AS [dbo];

