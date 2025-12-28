CREATE PROCEDURE [dbo].[admsvr_LoadServiceProperties]
@uidServiceID	uniqueidentifier,
@nvcAppName		nvarchar(80)
AS
SELECT	LowWatermark, HighWatermark, BatchSize, SingleDequeueSession, SerializeInstanceDelivery, GroupBatchByInstance, LowMemorymark, HighMemorymark, ThrottleFlag, LowSessionmark, HighSessionmark, CacheInstanceState, MaxDequeueThread,
		CAST(MessageDeliverySampleSpaceSize AS int), CAST(MessageDeliverySampleSpaceWindow AS int), CAST(MessageDeliveryOverdriveFactor AS int), CAST(MessageDeliveryMaximumDelay AS int), 
		CAST(MessagePublishSampleSpaceSize AS int), CAST(MessagePublishSampleSpaceWindow AS int), CAST(MessagePublishOverdriveFactor AS int), CAST(MessagePublishMaximumDelay AS int), 
		CAST(DeliveryQueueSize AS int), CAST(DBSessionThreshold AS int), CAST(GlobalMemoryThreshold AS int), CAST(ProcessMemoryThreshold  AS int), CAST(ThreadThreshold AS int), 
		CAST(DBQueueSizeThreshold AS int), CAST(InflightMessageThreshold AS int), 0
		FROM	adm_ServiceClass, adm_Host
		
WHERE	adm_ServiceClass.UniqueId = CAST( @uidServiceID AS NVARCHAR(256) ) AND adm_Host.Name = @nvcAppName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_LoadServiceProperties] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_LoadServiceProperties] TO [BTS_OPERATORS]
    AS [dbo];

