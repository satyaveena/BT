create procedure [dbo].[bam_ID479A949D030742C988E0992371EBC6FD_ID9F4DF99B98C64B5D9E604D221A620FDD_GetUpdated]
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
            from [dbo].[bam_EmailFromFile_Completed] with (index (NCI_LastModified))
            where (LastModified>@Start)
        end

        
        select @MaxRecordID = IDENT_CURRENT(N'[dbo].[bam_EmailFromFile_Completed]')

        if (@MaxRecordID = 1)
        begin
          if (not exists (select * from [dbo].[bam_EmailFromFile_Completed]))
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
  
    [EmailSent],   
    
    [FileReceived],   
    
    [EmaiRecipientAdded],   
    
    
    [ProcessTime],   
    
    [EmailConstructed],   
    
    
        
    [EmailProgress]    
    
    
    
   from [dbo].[bam_EmailProcess_ViewEmailFromFile_CompletedView]
   
   where (@RecordIDToUse is not null) and (RecordID>@RecordIDToUse) and (RecordID<=@MaxRecordID)

  end
  else
  begin
  set @LastRec=null
  
  select  null as 'TemporalID', ActivityID,
  
    [EmailSent],
  
    [FileReceived],
  
    [EmaiRecipientAdded],
  
  
    [ProcessTime],
  
    [EmailConstructed],
  

  
    [EmailProgress] 
  
  

  from [dbo].[bam_EmailProcess_ViewEmailFromFile_ActiveView]
  where ( LastModified>@Start ) and (LastModified<=@End)
  
  end
  

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID479A949D030742C988E0992371EBC6FD_ID9F4DF99B98C64B5D9E604D221A620FDD_GetUpdated] TO [BAM_ManagementNSReader]
    AS [dbo];

