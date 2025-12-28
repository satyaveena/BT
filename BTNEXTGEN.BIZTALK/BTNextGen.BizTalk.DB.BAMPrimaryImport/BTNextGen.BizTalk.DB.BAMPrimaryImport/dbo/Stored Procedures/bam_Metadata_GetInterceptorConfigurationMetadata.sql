-- Get list of activities enabled by interception and corresponding event sources
CREATE PROCEDURE [dbo].[bam_Metadata_GetInterceptorConfigurationMetadata]
AS
	SELECT ES.EventSourceName, ES.TechnologyName, ES.Manifest, IC.ActivityName, IC.Operator, IC.TimeCreated
	FROM [dbo].[bam_Metadata_EventSource] AS ES WITH (SERIALIZABLE)
	JOIN [dbo].[bam_Metadata_InterceptorConfiguration] AS IC WITH (SERIALIZABLE) ON IC.EventSourceName = ES.EventSourceName
	JOIN
	(SELECT IC2.EventSourceName, MAX(Version) AS MaxVersion
	FROM [dbo].[bam_Metadata_InterceptorConfiguration] AS IC2
	GROUP BY EventSourceName) AS MAXIC ON IC.EventSourceName = MAXIC.EventSourceName AND IC.Version = MAXIC.MaxVersion
	ORDER BY IC.ActivityName