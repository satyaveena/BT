create procedure [dbo].[bts_MarkAndGetOrphanFunctionalAckRecords]
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
	where activityname = 'FunctionalAckActivity' AND ArchivedTime is NULL

	insert into @myTable (InstancesTable) values('bam_FunctionalAckActivity_Completed')
		
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
				set @query = 'select faa.TimeCreated, faa.GroupControlNo, faa.ReceiverID, faa.SenderID, faa.ReceiverQ, faa.SenderQ, 
				faa.InterchangeDateTime, faa.Direction, faa.activityID  
			from ' + @instancetablename + ' as faa
			left outer join 
				(
					select isa.InterchangeControlNo, isa.ReceiverID, isa.SenderID, isa.ReceiverQ, isa.SenderQ, 
						isa.InterchangeDateTime, isa.Direction, fga.GroupControlNo
					from bam_InterchangeStatusActivity_CompletedInstances isa inner join
						bam_FunctionalGroupInfo_CompletedInstances fga
					on
						fga.InterchangeActivityID = isa.ActivityID
				) fgisa
			ON faa.GroupControlNo = fgisa.GroupControlNo AND faa.ReceiverID = fgisa.SenderID AND faa.SenderID = fgisa.ReceiverID 
				AND faa.ReceiverQ = fgisa.SenderQ AND faa.SenderQ = fgisa.ReceiverQ 
				AND faa.Direction != fgisa.Direction
			Where datediff(mi, faa.TimeCreated, getutcdate()) > @OnlineDurationInMinutes AND fgisa.InterchangeControlNo IS NULL AND 
				fgisa.ReceiverID IS NULL AND fgisa.SenderID IS NULL AND 
				fgisa.ReceiverQ IS NULL AND fgisa.SenderQ IS NULL AND 
				fgisa.Direction IS NULL AND (faa.RowFlags & 0x10) = 0'
		exec sp_executesql @query, N'@OnlineDurationInMinutes int', @OnlineDurationInMinutes
		
		set @query = 'Update ' + @instancetablename + '  SET RowFlags = (faa.RowFlags | 0x10)
			from ' + @instancetablename + ' as faa
			left outer join 
				(
					select isa.InterchangeControlNo, isa.ReceiverID, isa.SenderID, isa.ReceiverQ, isa.SenderQ, 
						isa.InterchangeDateTime, isa.Direction, fga.GroupControlNo
					from bam_InterchangeStatusActivity_CompletedInstances isa inner join
						bam_FunctionalGroupInfo_CompletedInstances fga
					on
						fga.InterchangeActivityID = isa.ActivityID
				) fgisa
			ON faa.GroupControlNo = fgisa.GroupControlNo AND faa.ReceiverID = fgisa.SenderID AND faa.SenderID = fgisa.ReceiverID 
				AND faa.ReceiverQ = fgisa.SenderQ AND faa.SenderQ = fgisa.ReceiverQ 
				AND faa.Direction != fgisa.Direction
			Where datediff(mi, faa.TimeCreated, getutcdate()) > @OnlineDurationInMinutes AND fgisa.InterchangeControlNo IS NULL AND 
				fgisa.ReceiverID IS NULL AND fgisa.SenderID IS NULL AND 
				fgisa.ReceiverQ IS NULL AND fgisa.SenderQ IS NULL AND 
				fgisa.Direction IS NULL AND (faa.RowFlags & 0x10) = 0'
		exec sp_executesql @query, N'@OnlineDurationInMinutes int', @OnlineDurationInMinutes
	
	   	   set @RowId = @RowId + 1
	
	end
END