-- Add new event source.  If event source name exists, delete it.
CREATE PROCEDURE [dbo].[bam_Metadata_AddEventSource]
(
	@eventSourceName NVARCHAR(128),
	@technologyName NVARCHAR(10),
	@manifest NVARCHAR(440),
	@eventSourceXml NTEXT
)
AS
	DECLARE @version INT
	DECLARE @eventSourceNameTemp NVARCHAR(128)

	-- Persist starting from last version for this (technology, manifest)
	SELECT @eventSourceNameTemp=EventSourceName, @version=Version FROM [dbo].[bam_Metadata_EventSource] WITH (SERIALIZABLE) WHERE TechnologyName = @technologyName AND Manifest = @manifest

	IF @version IS NULL 
	BEGIN -- Initially set version to zero since AddIC SP will increment
		SET @version = 0
	END
	ELSE
	BEGIN 
		-- If this (technology, manifest) is not being referenced by IC table, delete this unused event source
		IF NOT EXISTS(SELECT TOP 1 * FROM [dbo].[bam_Metadata_InterceptorConfiguration] WHERE EventSourceName = @eventSourceNameTemp)
		BEGIN
			DELETE FROM [dbo].[bam_Metadata_EventSource] WHERE EventSourceName = @eventSourceNameTemp
		END
	END

	-- If necessary delete event source with same name (Force was used).  
	IF EXISTS(SELECT TOP 1 * FROM [dbo].[bam_Metadata_EventSource] WHERE EventSourceName = @eventSourceName)
	BEGIN 
		DELETE FROM [dbo].[bam_Metadata_EventSource] WHERE EventSourceName = @eventSourceName
	END

	INSERT INTO [dbo].[bam_Metadata_EventSource] 
	(
		EventSourceName,
		TechnologyName,
		Manifest,
		EventSourceXml,
		Version
	) 
	VALUES 
	(
		@eventSourceName,
		@technologyName,
		@manifest,
		@eventSourceXml,
		@version
	)