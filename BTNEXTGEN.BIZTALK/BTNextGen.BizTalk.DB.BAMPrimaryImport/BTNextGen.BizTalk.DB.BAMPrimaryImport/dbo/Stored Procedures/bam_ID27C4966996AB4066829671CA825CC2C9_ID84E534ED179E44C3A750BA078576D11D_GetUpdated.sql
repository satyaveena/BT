create procedure [dbo].[bam_ID27C4966996AB4066829671CA825CC2C9_ID84E534ED179E44C3A750BA078576D11D_GetUpdated]
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
            from [dbo].[bam_PaymentERPTxn_Completed] with (index (NCI_LastModified))
            where (LastModified>@Start)
        end

        
        select @MaxRecordID = IDENT_CURRENT(N'[dbo].[bam_PaymentERPTxn_Completed]')

        if (@MaxRecordID = 1)
        begin
          if (not exists (select * from [dbo].[bam_PaymentERPTxn_Completed]))
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
  
    [cardID],   
    
    [email],   
    
    [GetTokenReq],   
    
    [MerchantRef],   
    
    [ProfileReq],   
    
    [ProfileResp],   
    
    [ERPSent],   
    
    [rcvCyber],   
    
    [rcvERPReq],   
    
    [Reason],   
    
    [SendCyber],   
    
    [SndTokenReq],   
    
    [TargetERP]    
    
    
    
    
    
    
   from [dbo].[bam_PaymentERPTxn_ViewPaymentERPTxn_CompletedView]
   
   where (@RecordIDToUse is not null) and (RecordID>@RecordIDToUse) and (RecordID<=@MaxRecordID)

  end
  else
  begin
  set @LastRec=null
  
  select  null as 'TemporalID', ActivityID,
  
    [cardID],
  
    [email],
  
    [GetTokenReq],
  
    [MerchantRef],
  
    [ProfileReq],
  
    [ProfileResp],
  
    [ERPSent],
  
    [rcvCyber],
  
    [rcvERPReq],
  
    [Reason],
  
    [SendCyber],
  
    [SndTokenReq],
  
    [TargetERP] 
  
  

  
  

  from [dbo].[bam_PaymentERPTxn_ViewPaymentERPTxn_ActiveView]
  where ( LastModified>@Start ) and (LastModified<=@End)
  
  end
  

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID27C4966996AB4066829671CA825CC2C9_ID84E534ED179E44C3A750BA078576D11D_GetUpdated] TO [BAM_ManagementNSReader]
    AS [dbo];

