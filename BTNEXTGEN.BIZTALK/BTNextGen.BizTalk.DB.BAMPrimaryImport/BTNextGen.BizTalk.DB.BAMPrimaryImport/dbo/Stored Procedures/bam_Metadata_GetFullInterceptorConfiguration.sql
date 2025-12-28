-- Get interceptor configuration to be used by deployment tool
CREATE PROCEDURE [dbo].[bam_Metadata_GetFullInterceptorConfiguration]
(
	@activityName NVARCHAR(128)=NULL,
	@eventSourceName NVARCHAR(128)=NULL
)
AS
	IF (@eventSourceName IS NULL AND @activityName IS NULL) -- Get IC for all
	BEGIN
		SELECT IC.EventSourceName, IC.ActivityName, IC.Version, IC.Operator, IC.TimeCreated, IC.OnEventXml, ES.EventSourceXml
		FROM [dbo].[bam_Metadata_EventSource] AS ES WITH (SERIALIZABLE)
		JOIN [dbo].[bam_Metadata_InterceptorConfiguration] AS IC WITH (SERIALIZABLE) ON IC.EventSourceName = ES.EventSourceName
		JOIN 
		(SELECT IC2.EventSourceName, MAX(Version) AS MaxVersion
		FROM [dbo].[bam_Metadata_InterceptorConfiguration] AS IC2
		GROUP BY EventSourceName) AS MAXIC ON IC.EventSourceName = MAXIC.EventSourceName AND IC.Version = MAXIC.MaxVersion
	END
	ELSE IF (@eventSourceName IS NULL) -- Get IC for activity
	BEGIN
		SELECT IC.EventSourceName, IC.ActivityName, IC.Version, IC.Operator, IC.TimeCreated, IC.OnEventXml, ES.EventSourceXml
		FROM [dbo].[bam_Metadata_EventSource] AS ES WITH (SERIALIZABLE)
		JOIN [dbo].[bam_Metadata_InterceptorConfiguration] AS IC WITH (SERIALIZABLE) ON IC.EventSourceName = ES.EventSourceName
		JOIN 
		(SELECT IC2.EventSourceName, MAX(Version) AS MaxVersion
		FROM [dbo].[bam_Metadata_InterceptorConfiguration] AS IC2
		WHERE ActivityName = @activityName 
		GROUP BY EventSourceName, ActivityName) AS MAXIC ON IC.EventSourceName = MAXIC.EventSourceName AND IC.Version = MAXIC.MaxVersion
		WHERE IC.ActivityName = @activityName 		
	END
	ELSE IF (@activityName IS NULL) -- Get IC for event source
	BEGIN
		SELECT IC.EventSourceName, IC.ActivityName, IC.Version, IC.Operator, IC.TimeCreated, IC.OnEventXml, ES.EventSourceXml
		FROM [dbo].[bam_Metadata_EventSource] AS ES WITH (SERIALIZABLE)
		JOIN [dbo].[bam_Metadata_InterceptorConfiguration] AS IC WITH (SERIALIZABLE) ON IC.EventSourceName = ES.EventSourceName
		JOIN 
		(SELECT MAX(Version) AS MaxVersion
		FROM [dbo].[bam_Metadata_InterceptorConfiguration] AS IC2
		WHERE EventSourceName = @eventSourceName) AS MAXIC ON IC.Version = MAXIC.MaxVersion
		WHERE IC.EventSourceName = @eventSourceName 			
	END
	ELSE -- Get IC for activity/event source
	BEGIN
		SELECT TOP 1 IC.EventSourceName, IC.ActivityName, IC.Version, IC.Operator, IC.TimeCreated, IC.OnEventXml, ES.EventSourceXml
		FROM [dbo].[bam_Metadata_EventSource] AS ES WITH (SERIALIZABLE)
		JOIN [dbo].[bam_Metadata_InterceptorConfiguration] AS IC WITH (SERIALIZABLE)
		ON IC.EventSourceName = ES.EventSourceName
		WHERE IC.EventSourceName = @eventSourceName 
		AND IC.ActivityName = @activityName 
		ORDER BY IC.Version DESC
	END