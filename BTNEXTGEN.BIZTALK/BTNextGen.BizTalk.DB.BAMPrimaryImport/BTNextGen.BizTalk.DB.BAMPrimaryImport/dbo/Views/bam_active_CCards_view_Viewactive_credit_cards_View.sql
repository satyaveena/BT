CREATE VIEW dbo.[bam_active_CCards_view_Viewactive_credit_cards_View]
        AS
          
          SELECT * FROM dbo.[bam_active_CCards_view_Viewactive_credit_cards_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_active_CCards_view_Viewactive_credit_cards_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_active_CCards_view_Viewactive_credit_cards_View] TO [bam_active_CCards_view]
    AS [dbo];

