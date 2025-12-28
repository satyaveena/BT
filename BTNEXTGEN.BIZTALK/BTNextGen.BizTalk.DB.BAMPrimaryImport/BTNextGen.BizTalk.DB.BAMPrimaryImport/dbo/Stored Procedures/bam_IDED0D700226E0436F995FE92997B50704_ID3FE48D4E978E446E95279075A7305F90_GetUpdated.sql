create procedure [dbo].[bam_IDED0D700226E0436F995FE92997B50704_ID3FE48D4E978E446E95279075A7305F90_GetUpdated]
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
  
    [EmailRecipient],   
    
    [EmailSender],   
    
    [EmailSent],   
    
    [FileReceived]    
    
    
    
    
    
    
   from [dbo].[bam_EmailTotals_ViewEmailFromFile_CompletedView]
   
   where (@RecordIDToUse is not null) and (RecordID>@RecordIDToUse) and (RecordID<=@MaxRecordID)

  end
  else
  begin
  set @LastRec=null
  
  select  null as 'TemporalID', ActivityID,
  
    [EmailRecipient],
  
    [EmailSender],
  
    [EmailSent],
  
    [FileReceived] 
  
  

  
  

  from [dbo].[bam_EmailTotals_ViewEmailFromFile_ActiveView]
  where ( LastModified>@Start ) and (LastModified<=@End)
  
  end
  

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_IDED0D700226E0436F995FE92997B50704_ID3FE48D4E978E446E95279075A7305F90_GetUpdated] TO [BAM_ManagementNSReader]
    AS [dbo];

