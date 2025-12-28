CREATE VIEW [dbo].[bam_ResendJournalActivity_ActiveInstances] AS SELECT NULL [RecordID]
, [ActivityID]
, [TrackingActivityID]
, [ResendIndex]
, [ResendStatus]
, [BTSInterchangeID]
, [LastModified]
 FROM [dbo].[bam_ResendJournalActivity_Active] WITH (NOLOCK) WHERE IsVisible = 1