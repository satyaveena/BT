CREATE VIEW [dbo].[bam_ResendTrackingActivity_ActiveInstances] AS SELECT NULL [RecordID]
, [ActivityID]
, [CorrelationId]
, [AdapterPrefix]
, [ResendCount]
, [MaxResendCount]
, [ResendInterval]
, [MaxRetryCount]
, [RetryInterval]
, [MessageContentId]
, [ResendTimeout]
, [RetryTimeout]
, [BTSInterchangeID]
, [LastModified]
 FROM [dbo].[bam_ResendTrackingActivity_Active] WITH (NOLOCK) WHERE IsVisible = 1
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_ResendTrackingActivity_ActiveInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_ResendTrackingActivity_ActiveInstances] TO [BTS_OPERATORS]
    AS [dbo];

