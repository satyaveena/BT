CREATE VIEW dbo.[bam_FirstDataReconView_ViewFirstDataRecon_CompletedView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [ProcessAttachment],
        [SendTolas]    
        -- Duration 
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_FirstDataReconView_ViewFirstDataRecon_CompletedAliasView]
      