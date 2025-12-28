create procedure [dbo].[bam_ID119D6D03461B4A3E905B8E25617F6AF6_IDE92B96A652974BDBAC8A03526D384D59_GetUpdated]
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
            from [dbo].[bam_Cybersource Settlement Feed_Completed] with (index (NCI_LastModified))
            where (LastModified>@Start)
        end

        
        select @MaxRecordID = IDENT_CURRENT(N'[dbo].[bam_Cybersource Settlement Feed_Completed]')

        if (@MaxRecordID = 1)
        begin
          if (not exists (select * from [dbo].[bam_Cybersource Settlement Feed_Completed]))
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
  
    [TimeEnded],   
    
    [TimeStarted]    
    
    
    
    
    
    
   from [dbo].[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_CompletedView]
   
   where (@RecordIDToUse is not null) and (RecordID>@RecordIDToUse) and (RecordID<=@MaxRecordID)

  end
  else
  begin
  set @LastRec=null
  
  select  null as 'TemporalID', ActivityID,
  
    [TimeEnded],
  
    [TimeStarted] 
  
  

  
  

  from [dbo].[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_ActiveView]
  where ( LastModified>@Start ) and (LastModified<=@End)
  
  end
  

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID119D6D03461B4A3E905B8E25617F6AF6_IDE92B96A652974BDBAC8A03526D384D59_GetUpdated] TO [BAM_ManagementNSReader]
    AS [dbo];

