CREATE VIEW dbo.[bam_EmailTotals_ViewEmailFromFile_CompletedAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [EmailRecipient] = 
                [EmailRecipient]
            ,
        [EmailSender] = 
                [EmailSender]
            ,
        [EmailSent] = 
                [EmailSent]
            ,
        [FileReceived] = 
                [FileReceived]
                
        -- Duration 

          FROM dbo.[bam_EmailFromFile_CompletedInstances]
      