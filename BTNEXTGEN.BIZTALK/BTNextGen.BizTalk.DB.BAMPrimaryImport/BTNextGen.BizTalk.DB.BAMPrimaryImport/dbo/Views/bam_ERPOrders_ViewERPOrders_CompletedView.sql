CREATE VIEW dbo.[bam_ERPOrders_ViewERPOrders_CompletedView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [AccountNum],
        [CSAckUpdate],
        [ERPAckRcv],
        [NGRHeaderUpdate],
        [NGRLineUpdate],
        [PONum],
        [PORcvd],
        [POSentERP],
        [TargetERP],
        [TransNum]    
        -- Duration 
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_ERPOrders_ViewERPOrders_CompletedAliasView]
      