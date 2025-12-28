CREATE VIEW dbo.[bam_PaymentERPTxn_ViewPaymentERPTxn_CompletedAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [cardID] = 
                [cardID]
            ,
        [email] = 
                [email]
            ,
        [GetTokenReq] = 
                [GetTokenReq]
            ,
        [MerchantRef] = 
                [MerchantRef]
            ,
        [ProfileReq] = 
                [ProfileReq]
            ,
        [ProfileResp] = 
                [ProfileResp]
            ,
        [ERPSent] = 
                [ERPSent]
            ,
        [rcvCyber] = 
                [rcvCyber]
            ,
        [rcvERPReq] = 
                [rcvERPReq]
            ,
        [Reason] = 
                [Reason]
            ,
        [SendCyber] = 
                [SendCyber]
            ,
        [SndTokenReq] = 
                [SndTokenReq]
            ,
        [TargetERP] = 
                [TargetERP]
                
        -- Duration 

          FROM dbo.[bam_PaymentERPTxn_CompletedInstances]
      