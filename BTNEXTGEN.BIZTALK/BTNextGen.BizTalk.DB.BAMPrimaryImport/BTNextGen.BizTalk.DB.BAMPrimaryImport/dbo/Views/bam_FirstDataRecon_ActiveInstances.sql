CREATE VIEW [dbo].[bam_FirstDataRecon_ActiveInstances] AS SELECT NULL [RecordID]
, [ActivityID]
, [ProcessAttachment]
, [SendTolas]
, [LastModified]
 FROM [dbo].[bam_FirstDataRecon_Active] WITH (NOLOCK) WHERE IsVisible = 1