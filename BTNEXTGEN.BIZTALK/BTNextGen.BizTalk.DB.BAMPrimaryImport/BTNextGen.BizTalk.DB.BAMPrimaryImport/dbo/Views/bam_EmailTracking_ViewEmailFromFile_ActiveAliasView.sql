CREATE VIEW dbo.[bam_EmailTracking_ViewEmailFromFile_ActiveAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [EmailConstructed] = 
                [EmailConstructed]
            ,
        [EmailRecipient] = 
                [EmailRecipient]
            ,
        [EmailSender] = 
                [EmailSender]
            ,
        [EmailSent] = 
                [EmailSent]
            ,
        [EmailServer] = 
                [EmailServer]
            ,
        [EmaiRecipientAdded] = 
                [EmaiRecipientAdded]
            ,
        [FileContent3885] = 
                [FileContent3885]
            ,
        [FileReceived] = 
                [FileReceived]
            ,
        [ProcessComplete] = 
                COALESCE(
                    [EmailConstructed], 
                    [EmailSent], 
                    [EmaiRecipientAdded], 
                    [FileReceived])
            ,    
        -- Duration 
        [FileToConstruct] = (CAST(
                [EmailConstructed]
             AS FLOAT) - 
            CAST(
                [FileReceived]
             AS FLOAT))*86400,
        [ProcessTime] = (CAST(
                [EmailSent]
             AS FLOAT) - 
            CAST(
                [FileReceived]
             AS FLOAT))*86400

          FROM dbo.[bam_EmailFromFile_ActiveInstances]    
      