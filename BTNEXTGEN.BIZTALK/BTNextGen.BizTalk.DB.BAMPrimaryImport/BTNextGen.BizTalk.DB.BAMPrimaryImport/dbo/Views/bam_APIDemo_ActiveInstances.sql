CREATE VIEW [dbo].[bam_APIDemo_ActiveInstances] AS SELECT NULL [RecordID]
, [ActivityID]
, [Key]
, [Data1]
, [Data2]
, [Data3]
, [LastModified]
 FROM [dbo].[bam_APIDemo_Active] WITH (NOLOCK) WHERE IsVisible = 1