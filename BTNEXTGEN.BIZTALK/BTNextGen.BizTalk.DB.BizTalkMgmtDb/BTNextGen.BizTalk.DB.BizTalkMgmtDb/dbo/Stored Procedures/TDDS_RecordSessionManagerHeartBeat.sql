create procedure [dbo].[TDDS_RecordSessionManagerHeartBeat]
(
	@SourceID uniqueidentifier = null,
	@ServiceID uniqueidentifier,
	@RecordsProcessed int = null,
	@Latency float = null,
	@EventsProcessed int = null,
	@RecordsLeft int=null,
	@ErrorCode int=null,
	@ErrorDescription nvarchar(1024)=null
)
as
begin
	declare @@HBCount int
	declare @@dummy uniqueidentifier
	
	begin transaction
	
	select top 1 @@dummy=ServiceID from TDDS_Heartbeats WITH  (TABLOCKX)
	
	
	if (@SourceID is null)
	begin
		select @@HBCount=Count(*)
		FROM TDDS_Heartbeats 
		where (SourceID is null) And (ServiceID=@ServiceID)		
	end
	else
	begin
		select @@HBCount=Count(*)
		FROM TDDS_Heartbeats 
		where (SourceID=@SourceID) And (ServiceID=@ServiceID)
	end
	
	if (@@HBCount>9)
	begin
		if (@SourceID is null)
		begin
			delete TDDS_Heartbeats  where Age = 
			( select min(Age) from TDDS_Heartbeats where (SourceID is null) and (ServiceID = @ServiceID ) )
		end
		else
		begin
			delete TDDS_Heartbeats where Age = 
			( select min(Age) from TDDS_Heartbeats where (SourceID = @SourceID) and (ServiceID = @ServiceID ))
		end
	end
	insert TDDS_Heartbeats
	(
		ServiceID,
		SourceID,
		TimeLastChanged,
		RecordsProcessed,
		Latency,
		EventsProcessed,
		RecordsLeft,
		ErrorCode,
		ErrorDescription
	)
	values
	(
		@ServiceID,
		@SourceID,
		GETUTCDATE(),
		@RecordsProcessed,
		@Latency,
		@EventsProcessed,
		@RecordsLeft,
		@ErrorCode,
		@ErrorDescription
	)
	commit transaction
	return 0	
	
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_RecordSessionManagerHeartBeat] TO [BAM_CONFIG_READER]
    AS [dbo];

