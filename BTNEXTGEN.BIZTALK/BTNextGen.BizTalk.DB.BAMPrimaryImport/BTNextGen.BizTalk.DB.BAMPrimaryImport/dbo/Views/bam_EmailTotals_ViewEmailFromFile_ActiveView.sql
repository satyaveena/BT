CREATE VIEW dbo.[bam_EmailTotals_ViewEmailFromFile_ActiveView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [EmailRecipient],
        [EmailSender],
        [EmailSent],
        [FileReceived]    
        -- Duration 
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_EmailTotals_ViewEmailFromFile_ActiveAliasView]
      