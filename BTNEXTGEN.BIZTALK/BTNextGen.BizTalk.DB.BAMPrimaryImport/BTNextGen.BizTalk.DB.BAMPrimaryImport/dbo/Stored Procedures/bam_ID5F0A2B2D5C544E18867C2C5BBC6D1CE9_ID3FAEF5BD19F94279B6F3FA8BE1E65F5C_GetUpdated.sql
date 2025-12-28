create procedure [dbo].[bam_ID5F0A2B2D5C544E18867C2C5BBC6D1CE9_ID3FAEF5BD19F94279B6F3FA8BE1E65F5C_GetUpdated]
        (
        @Start datetime,
        @End datetime,
        @RecID bigint,
        @CompletedBatchSize int,
        @GetCompleted bit, -- 0 is Query Active, 1 is Query Completed
        @LastRec int OUTPUT
        )
        as
        begin


      if ( @GetCompleted = 1)
      begin
        declare @RecordIDToUse bigint
        declare @MaxRecordID bigint
        
        set @RecordIDToUse=@RecID
       
        if (@RecordIDToUse is null)
        begin
            select @RecordIDToUse=Min(RecordID)-1
            from [dbo].[bam_active_credit_cards_Completed] with (index (NCI_LastModified))
            where (LastModified>@Start)
        end

        
        select @MaxRecordID = IDENT_CURRENT(N'[dbo].[bam_active_credit_cards_Completed]')

        if (@MaxRecordID = 1)
        begin
          if (not exists (select * from [dbo].[bam_active_credit_cards_Completed]))
          begin
            set @MaxRecordID = null
          end
        end
        
        if ((@MaxRecordID-@RecordIDToUse)>@CompletedBatchSize)
        begin
          set @MaxRecordID=@RecordIDToUse+@CompletedBatchSize
        end
        
        set @LastRec=@MaxRecordID

  select  null as 'TemporalID', ActivityID,
  
    [Alias],   
    
    [CardID],   
    
    [CSAckSent],   
    
    [CSCardRcv],   
    
    [Destination],   
    
    [ERPAccountNo],   
    
    [ERPSent]    
    
    
    
    
    
    
   from [dbo].[bam_active_CCards_view_Viewactive_credit_cards_CompletedView]
   
   where (@RecordIDToUse is not null) and (RecordID>@RecordIDToUse) and (RecordID<=@MaxRecordID)

  end
  else
  begin
  set @LastRec=null
  
  select  null as 'TemporalID', ActivityID,
  
    [Alias],
  
    [CardID],
  
    [CSAckSent],
  
    [CSCardRcv],
  
    [Destination],
  
    [ERPAccountNo],
  
    [ERPSent] 
  
  

  
  

  from [dbo].[bam_active_CCards_view_Viewactive_credit_cards_ActiveView]
  where ( LastModified>@Start ) and (LastModified<=@End)
  
  end
  

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID5F0A2B2D5C544E18867C2C5BBC6D1CE9_ID3FAEF5BD19F94279B6F3FA8BE1E65F5C_GetUpdated] TO [BAM_ManagementNSReader]
    AS [dbo];

