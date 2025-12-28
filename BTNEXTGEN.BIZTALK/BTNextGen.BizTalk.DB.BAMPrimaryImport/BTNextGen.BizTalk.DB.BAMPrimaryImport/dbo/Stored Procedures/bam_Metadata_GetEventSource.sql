-- Get event source.  Only return event sources that have corresponding entry in IC table.
CREATE PROCEDURE [dbo].[bam_Metadata_GetEventSource]
(
	@eventSourceName NVARCHAR(128)=NULL
)
AS	
	IF @eventSourceName IS NULL
	BEGIN
		SELECT ES.EventSourceName, ES.TechnologyName, ES.Manifest, ES.EventSourceXml
		FROM [dbo].[bam_Metadata_EventSource] AS ES JOIN [dbo].[bam_Metadata_InterceptorConfiguration] AS IC
		ON IC.EventSourceName = ES.EventSourceName
	END
	ELSE
	BEGIN	
		SELECT ES.EventSourceName, ES.TechnologyName, ES.Manifest, ES.EventSourceXml
		FROM [dbo].[bam_Metadata_EventSource] AS ES JOIN [dbo].[bam_Metadata_InterceptorConfiguration] AS IC
		ON ES.EventSourceName = @eventSourceName
	END