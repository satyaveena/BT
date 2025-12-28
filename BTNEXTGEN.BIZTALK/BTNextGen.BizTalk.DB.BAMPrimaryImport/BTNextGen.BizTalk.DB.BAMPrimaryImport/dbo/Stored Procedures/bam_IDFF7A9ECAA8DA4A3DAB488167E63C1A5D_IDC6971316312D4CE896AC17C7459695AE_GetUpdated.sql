create procedure [dbo].[bam_IDFF7A9ECAA8DA4A3DAB488167E63C1A5D_IDC6971316312D4CE896AC17C7459695AE_GetUpdated]
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
            from [dbo].[bam_ExpiredCreditCards_Completed] with (index (NCI_LastModified))
            where (LastModified>@Start)
        end

        
        select @MaxRecordID = IDENT_CURRENT(N'[dbo].[bam_ExpiredCreditCards_Completed]')

        if (@MaxRecordID = 1)
        begin
          if (not exists (select * from [dbo].[bam_ExpiredCreditCards_Completed]))
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
    
    [DateCreated],   
    
    [ExpirationMonth],   
    
    [ExpirationYear],   
    
    [ID_CreditCards],   
    
    [ID_UserObjects],   
    
    [Last4Digits],   
    
    [EmailAddress]    
    
    
    
    
    
    
   from [dbo].[bam_ExpiredCreditCards_ViewExpiredCreditCards_CompletedView]
   
   where (@RecordIDToUse is not null) and (RecordID>@RecordIDToUse) and (RecordID<=@MaxRecordID)

  end
  else
  begin
  set @LastRec=null
  
  select  null as 'TemporalID', ActivityID,
  
    [Alias],
  
    [DateCreated],
  
    [ExpirationMonth],
  
    [ExpirationYear],
  
    [ID_CreditCards],
  
    [ID_UserObjects],
  
    [Last4Digits],
  
    [EmailAddress] 
  
  

  
  

  from [dbo].[bam_ExpiredCreditCards_ViewExpiredCreditCards_ActiveView]
  where ( LastModified>@Start ) and (LastModified<=@End)
  
  end
  

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_IDFF7A9ECAA8DA4A3DAB488167E63C1A5D_IDC6971316312D4CE896AC17C7459695AE_GetUpdated] TO [BAM_ManagementNSReader]
    AS [dbo];

