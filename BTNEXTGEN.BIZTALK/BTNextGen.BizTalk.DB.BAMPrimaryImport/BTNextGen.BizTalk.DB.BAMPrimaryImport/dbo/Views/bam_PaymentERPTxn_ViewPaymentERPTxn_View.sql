CREATE VIEW dbo.[bam_PaymentERPTxn_ViewPaymentERPTxn_View]
        AS
          
          SELECT * FROM dbo.[bam_PaymentERPTxn_ViewPaymentERPTxn_ActiveView]
          UNION ALL
          SELECT * FROM dbo.[bam_PaymentERPTxn_ViewPaymentERPTxn_CompletedView]
      
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_PaymentERPTxn_ViewPaymentERPTxn_View] TO [bam_PaymentERPTxn]
    AS [dbo];

