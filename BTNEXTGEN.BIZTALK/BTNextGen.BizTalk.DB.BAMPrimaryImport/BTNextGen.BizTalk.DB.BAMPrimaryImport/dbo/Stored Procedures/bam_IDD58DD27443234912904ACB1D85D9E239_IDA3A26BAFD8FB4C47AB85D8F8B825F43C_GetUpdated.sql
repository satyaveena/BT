create procedure [dbo].[bam_IDD58DD27443234912904ACB1D85D9E239_IDA3A26BAFD8FB4C47AB85D8F8B825F43C_GetUpdated]
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
            from [dbo].[bam_APIDemo_Completed] with (index (NCI_LastModified))
            where (LastModified>@Start)
        end

        
        select @MaxRecordID = IDENT_CURRENT(N'[dbo].[bam_APIDemo_Completed]')

        if (@MaxRecordID = 1)
        begin
          if (not exists (select * from [dbo].[bam_APIDemo_Completed]))
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
  
    [Data1],   
    
    [Data2],   
    
    [Data3],   
    
    [Key]    
    
    
    
    
    
    
   from [dbo].[bam_APIDemo_ViewAPIDemo_CompletedView]
   
   where (@RecordIDToUse is not null) and (RecordID>@RecordIDToUse) and (RecordID<=@MaxRecordID)

  end
  else
  begin
  set @LastRec=null
  
  select  null as 'TemporalID', ActivityID,
  
    [Data1],
  
    [Data2],
  
    [Data3],
  
    [Key] 
  
  

  
  

  from [dbo].[bam_APIDemo_ViewAPIDemo_ActiveView]
  where ( LastModified>@Start ) and (LastModified<=@End)
  
  end
  

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_IDD58DD27443234912904ACB1D85D9E239_IDA3A26BAFD8FB4C47AB85D8F8B825F43C_GetUpdated] TO [BAM_ManagementNSReader]
    AS [dbo];

