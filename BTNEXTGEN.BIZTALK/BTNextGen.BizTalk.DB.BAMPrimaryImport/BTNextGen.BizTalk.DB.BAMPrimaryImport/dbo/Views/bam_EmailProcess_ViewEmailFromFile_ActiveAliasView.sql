CREATE VIEW dbo.[bam_EmailProcess_ViewEmailFromFile_ActiveAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [EmailSent] = 
                [EmailSent]
            ,
        [FileReceived] = 
                [FileReceived]
            ,
        [EmaiRecipientAdded] = 
                [EmaiRecipientAdded]
            ,    
        -- Duration 
        [ProcessTime] = (CAST(
                [EmailSent]
             AS FLOAT) - 
            CAST(
                [FileReceived]
             AS FLOAT))*86400,
        [EmailConstructed] = (CAST(
                [EmaiRecipientAdded]
             AS FLOAT) - 
            CAST(
                [FileReceived]
             AS FLOAT))*86400

          FROM dbo.[bam_EmailFromFile_ActiveInstances]    
      