CREATE VIEW [dbo].[bam_FunctionalGroupInfo_ActiveInstances] AS SELECT NULL [RecordID]
, [ActivityID]
, [InterchangeActivityID]
, [GroupControlNo]
, [FunctionalIDCode]
, [TSCount]
, [LastModified]
 FROM [dbo].[bam_FunctionalGroupInfo_Active] WITH (NOLOCK) WHERE IsVisible = 1