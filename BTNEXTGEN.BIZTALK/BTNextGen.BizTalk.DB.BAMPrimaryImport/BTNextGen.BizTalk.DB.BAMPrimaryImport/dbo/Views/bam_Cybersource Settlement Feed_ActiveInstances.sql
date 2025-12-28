CREATE VIEW [dbo].[bam_Cybersource Settlement Feed_ActiveInstances] AS SELECT NULL [RecordID]
, [ActivityID]
, [TimeStarted]
, [TimeEnded]
, [LastModified]
 FROM [dbo].[bam_Cybersource Settlement Feed_Active] WITH (NOLOCK) WHERE IsVisible = 1