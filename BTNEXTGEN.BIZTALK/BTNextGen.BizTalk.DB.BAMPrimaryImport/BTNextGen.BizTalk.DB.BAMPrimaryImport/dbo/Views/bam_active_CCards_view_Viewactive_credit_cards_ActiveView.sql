CREATE VIEW dbo.[bam_active_CCards_view_Viewactive_credit_cards_ActiveView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [Alias],
        [CardID],
        [CSAckSent],
        [CSCardRcv],
        [Destination],
        [ERPAccountNo],
        [ERPSent]    
        -- Duration 
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_active_CCards_view_Viewactive_credit_cards_ActiveAliasView]
      