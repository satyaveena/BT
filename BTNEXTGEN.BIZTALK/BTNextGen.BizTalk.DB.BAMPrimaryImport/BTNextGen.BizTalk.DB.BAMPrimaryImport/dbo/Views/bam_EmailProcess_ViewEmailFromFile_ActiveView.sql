CREATE VIEW dbo.[bam_EmailProcess_ViewEmailFromFile_ActiveView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [EmailSent],
        [FileReceived],
        [EmaiRecipientAdded],    
        -- Duration 
        [ProcessTime],
        [EmailConstructed],
        -- Computed range dimension columns         
        -- Computed progress dimension columns 
            [EmailProgress] =
            CASE 
                WHEN [EmailSent] IS NOT NULL THEN N'Sent'
                WHEN [EmaiRecipientAdded] IS NOT NULL THEN N'Sending'
                WHEN [FileReceived] IS NOT NULL THEN N'Construction'
            END
        

          FROM dbo.[bam_EmailProcess_ViewEmailFromFile_ActiveAliasView]
      