Create procedure [dbo].[bts_MarkAndGetOrphanInterchangeAckRecords] 
(
	@OnlineDurationInMinutes int
)
AS
BEGIN
	declare @myTable table 
	   (RowId int identity(1,1),
		InstancesTable sysname)
	
	declare @instancetablename sysname
	declare @RowId int, @MaxRowId int
	declare @query nvarchar(2000)
	
		insert into @myTable (InstancesTable)
	select InstancesTable
	from bam_metadata_partitions
	where activityname = 'InterchangeAckActivity' AND ArchivedTime is NULL

	insert into @myTable (InstancesTable) values('bam_InterchangeAckActivity_Completed')
		
	select 
	   @RowId = min(RowId),
	   @MaxRowId = max(RowId) 
	from @myTable
	
	while @RowId <= @MaxRowId begin
	   	   	   select @instancetablename = InstancesTable
	   from 
	      @myTable
	   where 
	      RowId = @RowId
	
		set @query = 'select iaa.TimeCreated, iaa.InterchangeControlNo, iaa.ReceiverID, iaa.SenderID, iaa.ReceiverQ, iaa.SenderQ, 
				iaa.InterchangeDateTime, iaa.Direction, iaa.activityID  
			from ' + @instancetablename + ' as iaa
			left outer join bam_InterchangeStatusActivity_CompletedInstances isa
			ON iaa.InterchangeControlNo = isa.InterchangeControlNo AND iaa.ReceiverID = isa.SenderID AND iaa.SenderID = isa.ReceiverID AND 
				iaa.ReceiverQ = isa.SenderQ AND iaa.SenderQ = isa.ReceiverQ AND 
				iaa.InterchangeDateTime = isa.InterchangeDateTime AND iaa.Direction != isa.Direction
			Where datediff(mi, iaa.TimeCreated, getutcdate()) > @OnlineDurationInMinutes AND isa.InterchangeControlNo IS NULL AND 
				isa.ReceiverID IS NULL AND isa.SenderID IS NULL AND isa.InterchangeDateTime IS NULL AND isa.Direction IS NULL AND
				isa.ReceiverQ IS NULL AND isa.SenderQ IS NULL AND 
				(iaa.RowFlags & 0x10) = 0'
		exec sp_executesql @query, N'@OnlineDurationInMinutes int', @OnlineDurationInMinutes
		
		set @query = 'Update ' + @instancetablename + '  SET RowFlags = (iaa.RowFlags | 0x10)
			from ' + @instancetablename + ' as iaa
			left outer join bam_InterchangeStatusActivity_CompletedInstances isa
			ON iaa.InterchangeControlNo = isa.InterchangeControlNo AND iaa.ReceiverID = isa.SenderID AND iaa.SenderID = isa.ReceiverID AND 
				iaa.ReceiverQ = isa.SenderQ AND iaa.SenderQ = isa.ReceiverQ AND 
				iaa.InterchangeDateTime = isa.InterchangeDateTime AND iaa.Direction != isa.Direction
			Where datediff(mi, iaa.TimeCreated, getutcdate()) > @OnlineDurationInMinutes AND isa.InterchangeControlNo IS NULL AND 
				isa.ReceiverID IS NULL AND isa.SenderID IS NULL AND isa.InterchangeDateTime IS NULL AND isa.Direction IS NULL AND
				isa.ReceiverQ IS NULL AND isa.SenderQ IS NULL AND 
				(iaa.RowFlags & 0x10) = 0'
		exec sp_executesql @query, N'@OnlineDurationInMinutes int', @OnlineDurationInMinutes
	
	   	   set @RowId = @RowId + 1
	
	end
END