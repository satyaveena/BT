CREATE VIEW dbo.[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_CompletedAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [TimeEnded] = 
                [TimeEnded]
            ,
        [TimeStarted] = 
                [TimeStarted]
                
        -- Duration 

          FROM dbo.[bam_Cybersource Settlement Feed_CompletedInstances]
      