-- Add new interceptor configuration
CREATE PROCEDURE [dbo].[bam_Metadata_AddInterceptorConfiguration]
(
	@eventSourceName NVARCHAR(128),
	@activityName NVARCHAR(128),
	@operator NVARCHAR(128),
	@onEventXml NTEXT
)
AS
	DECLARE @version INT

	-- Find version for this (EventSource, Activity).  Use Version (from ES table) + 1
	SELECT @version=Version FROM [dbo].[bam_Metadata_EventSource] WITH (ROWLOCK, XLOCK) WHERE EventSourceName = @eventSourceName

    -- Use next version	
	SET @version = @version + 1

	-- Create temporary variables for cursors
	DECLARE @eventSourceValue NVARCHAR(128)
	DECLARE @activityValue NVARCHAR(128)
	DECLARE @versionValue INT
	
	-- Find all other BAM activities that belong to this event source
	DECLARE eventsource_cursor CURSOR FOR
	SELECT EventSourceName, ActivityName, MAX(Version) FROM
	[dbo].[bam_Metadata_InterceptorConfiguration] WITH (SERIALIZABLE)
	WHERE EventSourceName = @eventSourceName 
	AND ActivityName <> @activityName
	GROUP BY EventSourceName, ActivityName

	OPEN eventsource_cursor
	FETCH NEXT FROM eventsource_cursor INTO @eventSourceValue, @activityValue, @versionValue

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- For each BAM activity, bump up the version by inserting a duplicate row with higher version
		INSERT [dbo].[bam_Metadata_InterceptorConfiguration]
		SELECT @eventSourceValue, @activityValue, @version, Operator, TimeCreated, OnEventXml 
		FROM [dbo].[bam_Metadata_InterceptorConfiguration]
		WHERE EventSourceName = @eventSourceValue 
		AND ActivityName = @activityValue
		AND Version = @versionValue

		FETCH NEXT FROM eventsource_cursor INTO @eventSourceValue, @activityValue, @versionValue
	END

	CLOSE eventsource_cursor
	DEALLOCATE eventsource_cursor

	-- Insert row for new XML belonging to this (EventSource, Activity)
	INSERT INTO [dbo].[bam_Metadata_InterceptorConfiguration] 
	(
		EventSourceName,
		ActivityName,
		Version,
		Operator,
		OnEventXml		
	) 
	VALUES 
	(
		@eventSourceName,
		@activityName,
		@version,
		@operator,
		@onEventXml
	)

    -- Update ES table with latest version
	UPDATE [dbo].[bam_Metadata_EventSource] SET Version = @version WHERE EventSourceName = @eventSourceName