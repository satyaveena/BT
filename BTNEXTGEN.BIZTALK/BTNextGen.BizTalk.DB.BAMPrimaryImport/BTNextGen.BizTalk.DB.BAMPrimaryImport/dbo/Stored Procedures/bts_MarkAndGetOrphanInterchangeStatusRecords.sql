Create procedure [dbo].[bts_MarkAndGetOrphanInterchangeStatusRecords]
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
	declare @query nvarchar(3000)
	
		insert into @myTable (InstancesTable)
	select InstancesTable
	from bam_metadata_partitions
	where activityname = 'InterchangeStatusActivity' AND ArchivedTime is NULL

	insert into @myTable (InstancesTable) values('bam_InterchangeStatusActivity_Completed')
		
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

			
		set @query = 'select 1 ISAOrphanType, isa.TimeCreated, isa.InterchangeControlNo, isa.ReceiverID, isa.SenderID, 
					isa.ReceiverQ, isa.SenderQ, isa.InterchangeDateTime, isa.Direction 
			from ' + @instancetablename + ' as isa
			left outer join bam_InterchangeAckActivity_CompletedInstances iaa
			ON isa.InterchangeControlNo = iaa.InterchangeControlNo AND isa.ReceiverID = iaa.SenderID AND isa.SenderID = iaa.ReceiverID AND 
				isa.ReceiverQ = iaa.SenderQ AND isa.SenderQ = iaa.ReceiverQ AND 
				isa.InterchangeDateTime = iaa.InterchangeDateTime AND isa.Direction != iaa.Direction
			Where datediff(mi, isa.TimeCreated, getutcdate()) > @OnlineDurationInMinutes AND iaa.InterchangeControlNo IS NULL AND 
				iaa.ReceiverID IS NULL AND iaa.SenderID IS NULL AND iaa.InterchangeDateTime IS NULL AND iaa.Direction IS NULL AND
				iaa.ReceiverQ IS NULL AND iaa.SenderQ IS NULL AND 
				(isa.RowFlags & 0x10) = 0 AND (isa.IsInterchangeAckExpected <> 0)'

		exec sp_executesql @query, N'@OnlineDurationInMinutes int', @OnlineDurationInMinutes
		
		set @query = 'Update ' + @instancetablename + '  SET RowFlags = (isa.RowFlags | 0x10)
			from ' + @instancetablename + ' as isa
			left outer join bam_InterchangeAckActivity_CompletedInstances iaa
			ON isa.InterchangeControlNo = iaa.InterchangeControlNo AND isa.ReceiverID = iaa.SenderID AND isa.SenderID = iaa.ReceiverID AND 
				isa.ReceiverQ = iaa.SenderQ AND isa.SenderQ = iaa.ReceiverQ AND 
				isa.InterchangeDateTime = iaa.InterchangeDateTime AND isa.Direction != iaa.Direction
			Where datediff(mi, isa.TimeCreated, getutcdate()) > @OnlineDurationInMinutes AND iaa.InterchangeControlNo IS NULL AND 
				iaa.ReceiverID IS NULL AND iaa.SenderID IS NULL AND iaa.InterchangeDateTime IS NULL AND iaa.Direction IS NULL AND
				iaa.ReceiverQ IS NULL AND iaa.SenderQ IS NULL AND 
				(isa.RowFlags & 0x10) = 0 AND (isa.IsInterchangeAckExpected <> 0)'
		exec sp_executesql @query, N'@OnlineDurationInMinutes int', @OnlineDurationInMinutes

			
		set @query = 
		'select 2 ISAOrphanType, fgisa.TimeCreated, fgisa.InterchangeControlNo, fgisa.ReceiverID, fgisa.SenderID, 
			fgisa.ReceiverQ, fgisa.SenderQ, fgisa.InterchangeDateTime, fgisa.Direction 
		from (
			select isa.InterchangeControlNo, isa.ReceiverID, isa.SenderID, isa.ReceiverQ, isa.SenderQ, 
				isa.InterchangeDateTime, isa.Direction, fga.GroupControlNo, isa.TimeCreated
			from ' + @instancetablename + ' isa inner join
				bam_FunctionalGroupInfo_CompletedInstances fga
			on
				fga.InterchangeActivityID = isa.ActivityID AND isa.IsFunctionalAckExpected <> 0 AND (isa.RowFlags & 0x10) = 0 
		) as fgisa
		left outer join  bam_FunctionalAckActivity_CompletedInstances faa
			ON fgisa.ReceiverID = faa.SenderID AND fgisa.SenderID = faa.ReceiverID AND 
				fgisa.ReceiverQ = faa.SenderQ AND fgisa.SenderQ = faa.ReceiverQ AND fgisa.InterchangeDateTime = faa.InterchangeDateTime AND
				fgisa.Direction != faa.Direction AND fgisa.GroupControlNo = faa.GroupControlNo 
			Where datediff(mi, fgisa.TimeCreated, getutcdate()) > @OnlineDurationInMinutes AND 
				faa.GroupControlNo Is NULL'

		exec sp_executesql @query, N'@OnlineDurationInMinutes int', @OnlineDurationInMinutes

		set @query = 'Update ' + @instancetablename + '  SET RowFlags = (RowFlags | 0x10) where activityID in 
			(select distinct fgisa.isaActivityID
			from (
			select isa.ActivityID isaActivityID, isa.InterchangeControlNo, isa.ReceiverID, isa.SenderID, isa.ReceiverQ, isa.SenderQ, 
				isa.InterchangeDateTime, isa.Direction, fga.GroupControlNo, isa.TimeCreated
			from ' + @instancetablename + ' isa inner join
				bam_FunctionalGroupInfo_CompletedInstances fga
			on
				fga.InterchangeActivityID = isa.ActivityID AND isa.IsFunctionalAckExpected <> 0 AND (isa.RowFlags & 0x10) = 0 
		) as fgisa
		left outer join  bam_FunctionalAckActivity_CompletedInstances faa
			ON fgisa.ReceiverID = faa.SenderID AND fgisa.SenderID = faa.ReceiverID AND 
				fgisa.ReceiverQ = faa.SenderQ AND fgisa.SenderQ = faa.ReceiverQ AND fgisa.InterchangeDateTime = faa.InterchangeDateTime AND
				fgisa.Direction != faa.Direction AND fgisa.GroupControlNo = faa.GroupControlNo 
			Where datediff(mi, fgisa.TimeCreated, getutcdate()) > @OnlineDurationInMinutes AND 
				faa.GroupControlNo Is NULL)'
		exec sp_executesql @query, N'@OnlineDurationInMinutes int', @OnlineDurationInMinutes

	   	   set @RowId = @RowId + 1
	
	end
END