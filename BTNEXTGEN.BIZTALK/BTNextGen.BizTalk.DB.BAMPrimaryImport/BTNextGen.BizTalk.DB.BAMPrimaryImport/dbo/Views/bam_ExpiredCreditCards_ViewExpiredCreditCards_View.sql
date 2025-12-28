CREATE VIEW dbo.[bam_ExpiredCreditCards_ViewExpiredCreditCards_View]
        AS
          
          SELECT * FROM dbo.[bam_ExpiredCreditCards_ViewExpiredCreditCards_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_ExpiredCreditCards_ViewExpiredCreditCards_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_ExpiredCreditCards_ViewExpiredCreditCards_View] TO [bam_ExpiredCreditCards]
    AS [dbo];

