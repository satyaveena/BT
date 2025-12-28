CREATE VIEW dbo.[bam_ExpiredCreditCards_ViewExpiredCreditCards_CompletedAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [Alias] = 
                [Alias]
            ,
        [DateCreated] = 
                [DateCreated]
            ,
        [ExpirationMonth] = 
                [ExpirationMonth]
            ,
        [ExpirationYear] = 
                [ExpirationYear]
            ,
        [ID_CreditCards] = 
                [ID_CreditCards]
            ,
        [ID_UserObjects] = 
                [ID_UserObjects]
            ,
        [Last4Digits] = 
                [Last4Digits]
            ,
        [EmailAddress] = 
                [EmailAddress]
                
        -- Duration 

          FROM dbo.[bam_ExpiredCreditCards_CompletedInstances]
      