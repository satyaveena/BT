CREATE VIEW dbo.[bam_PaymentERPTxn_ViewPaymentERPTxn_CompletedView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [cardID],
        [email],
        [GetTokenReq],
        [MerchantRef],
        [ProfileReq],
        [ProfileResp],
        [ERPSent],
        [rcvCyber],
        [rcvERPReq],
        [Reason],
        [SendCyber],
        [SndTokenReq],
        [TargetERP]    
        -- Duration 
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_PaymentERPTxn_ViewPaymentERPTxn_CompletedAliasView]
      