CREATE VIEW dbo.[bam_ERPOrders_ViewERPOrders_CompletedAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [AccountNum] = 
                [AccountNum]
            ,
        [CSAckUpdate] = 
                [CSAckUpdate]
            ,
        [ERPAckRcv] = 
                [ERPAckRcv]
            ,
        [NGRHeaderUpdate] = 
                [NGRHeaderUpdate]
            ,
        [NGRLineUpdate] = 
                [NGRLineUpdate]
            ,
        [PONum] = 
                [PONum]
            ,
        [PORcvd] = 
                [PORcvd]
            ,
        [POSentERP] = 
                [POSentERP]
            ,
        [TargetERP] = 
                [TargetERP]
            ,
        [TransNum] = 
                [TransNum]
                
        -- Duration 

          FROM dbo.[bam_ERPOrders_CompletedInstances]
      