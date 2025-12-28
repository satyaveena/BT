CREATE VIEW dbo.[bam_EmailTotals_CubingSnapshot]
        AS
          
      SELECT * FROM dbo.[bam_EmailTotals_ActiveInstancesSnapshot]
      UNION ALL
      SELECT * FROM dbo.[bam_EmailTotals_CompletedInstancesWindow]
    