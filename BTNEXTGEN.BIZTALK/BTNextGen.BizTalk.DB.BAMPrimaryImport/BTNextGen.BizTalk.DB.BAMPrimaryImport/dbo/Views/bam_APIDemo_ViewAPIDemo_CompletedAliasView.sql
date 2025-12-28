CREATE VIEW dbo.[bam_APIDemo_ViewAPIDemo_CompletedAliasView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [Data1] = 
                [Data1]
            ,
        [Data2] = 
                [Data2]
            ,
        [Data3] = 
                [Data3]
            ,
        [Key] = 
                [Key]
                
        -- Duration 

          FROM dbo.[bam_APIDemo_CompletedInstances]
      