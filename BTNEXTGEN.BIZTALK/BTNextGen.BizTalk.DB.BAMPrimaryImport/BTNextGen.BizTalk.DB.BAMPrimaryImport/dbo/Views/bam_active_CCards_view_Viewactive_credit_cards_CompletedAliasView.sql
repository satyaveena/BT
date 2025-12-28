CREATE VIEW dbo.[bam_active_CCards_view_Viewactive_credit_cards_CompletedAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [Alias] = 
                [Alias]
            ,
        [CardID] = 
                [CardID]
            ,
        [CSAckSent] = 
                [CSAckSent]
            ,
        [CSCardRcv] = 
                [CSCardRcv]
            ,
        [Destination] = 
                [Destination]
            ,
        [ERPAccountNo] = 
                [ERPAccountNo]
            ,
        [ERPSent] = 
                [ERPSent]
                
        -- Duration 

          FROM dbo.[bam_active_credit_cards_CompletedInstances]
      