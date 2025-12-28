CREATE VIEW dbo.[bam_ExpiredCreditCards_ViewExpiredCreditCards_ActiveView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [Alias],
        [DateCreated],
        [ExpirationMonth],
        [ExpirationYear],
        [ID_CreditCards],
        [ID_UserObjects],
        [Last4Digits],
        [EmailAddress]    
        -- Duration 
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_ExpiredCreditCards_ViewExpiredCreditCards_ActiveAliasView]
      