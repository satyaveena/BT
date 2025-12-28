CREATE VIEW dbo.[bam_FirstDataReconView_ViewFirstDataRecon_CompletedAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [ProcessAttachment] = 
                [ProcessAttachment]
            ,
        [SendTolas] = 
                [SendTolas]
                
        -- Duration 

          FROM dbo.[bam_FirstDataRecon_CompletedInstances]
      