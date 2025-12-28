CREATE VIEW dbo.[bam_EmailTracking_ViewEmailFromFile_CompletedView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [EmailConstructed],
        [EmailRecipient],
        [EmailSender],
        [EmailSent],
        [EmailServer],
        [EmaiRecipientAdded],
        [FileContent3885],
        [FileReceived],
        [ProcessComplete],    
        -- Duration 
        [FileToConstruct],
        [ProcessTime]
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_EmailTracking_ViewEmailFromFile_CompletedAliasView]
      