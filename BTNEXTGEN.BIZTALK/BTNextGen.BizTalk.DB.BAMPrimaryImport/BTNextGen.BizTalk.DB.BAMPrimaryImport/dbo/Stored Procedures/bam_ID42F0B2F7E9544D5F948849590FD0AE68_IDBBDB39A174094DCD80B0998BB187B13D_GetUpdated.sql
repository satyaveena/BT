create procedure [dbo].[bam_ID42F0B2F7E9544D5F948849590FD0AE68_IDBBDB39A174094DCD80B0998BB187B13D_GetUpdated]
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
            from [dbo].[bam_ERPOrders_Completed] with (index (NCI_LastModified))
            where (LastModified>@Start)
        end

        
        select @MaxRecordID = IDENT_CURRENT(N'[dbo].[bam_ERPOrders_Completed]')

        if (@MaxRecordID = 1)
        begin
          if (not exists (select * from [dbo].[bam_ERPOrders_Completed]))
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
  
    [AccountNum],   
    
    [CSAckUpdate],   
    
    [ERPAckRcv],   
    
    [NGRHeaderUpdate],   
    
    [NGRLineUpdate],   
    
    [PONum],   
    
    [PORcvd],   
    
    [POSentERP],   
    
    [TargetERP],   
    
    [TransNum]    
    
    
    
    
    
    
   from [dbo].[bam_ERPOrders_ViewERPOrders_CompletedView]
   
   where (@RecordIDToUse is not null) and (RecordID>@RecordIDToUse) and (RecordID<=@MaxRecordID)

  end
  else
  begin
  set @LastRec=null
  
  select  null as 'TemporalID', ActivityID,
  
    [AccountNum],
  
    [CSAckUpdate],
  
    [ERPAckRcv],
  
    [NGRHeaderUpdate],
  
    [NGRLineUpdate],
  
    [PONum],
  
    [PORcvd],
  
    [POSentERP],
  
    [TargetERP],
  
    [TransNum] 
  
  

  
  

  from [dbo].[bam_ERPOrders_ViewERPOrders_ActiveView]
  where ( LastModified>@Start ) and (LastModified<=@End)
  
  end
  

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID42F0B2F7E9544D5F948849590FD0AE68_IDBBDB39A174094DCD80B0998BB187B13D_GetUpdated] TO [BAM_ManagementNSReader]
    AS [dbo];

