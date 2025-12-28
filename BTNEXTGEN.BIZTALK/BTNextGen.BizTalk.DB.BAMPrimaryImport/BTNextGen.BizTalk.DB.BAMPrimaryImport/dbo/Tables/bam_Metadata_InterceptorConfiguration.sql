CREATE TABLE [dbo].[bam_Metadata_InterceptorConfiguration] (
    [EventSourceName] NVARCHAR (128) NOT NULL,
    [ActivityName]    NVARCHAR (128) NOT NULL,
    [Version]         INT            NOT NULL,
    [Operator]        NVARCHAR (128) NOT NULL,
    [TimeCreated]     DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [OnEventXml]      NTEXT          NOT NULL,
    FOREIGN KEY ([ActivityName]) REFERENCES [dbo].[bam_Metadata_Activities] ([ActivityName]) ON DELETE CASCADE,
    FOREIGN KEY ([EventSourceName]) REFERENCES [dbo].[bam_Metadata_EventSource] ([EventSourceName]) ON DELETE CASCADE
);


GO
CREATE UNIQUE CLUSTERED INDEX [InterceptorConfigurationIndex]
    ON [dbo].[bam_Metadata_InterceptorConfiguration]([EventSourceName] ASC, [ActivityName] ASC, [Version] ASC);


GO
CREATE NONCLUSTERED INDEX [InterceptorConfigurationActivityIndex]
    ON [dbo].[bam_Metadata_InterceptorConfiguration]([ActivityName] ASC);


GO
-- Delete trigger on interceptor configuration table to clean up event sources
CREATE TRIGGER bam_Metadata_CleanupEventSource
ON [dbo].[bam_Metadata_InterceptorConfiguration]
AFTER DELETE
AS
	DECLARE @eventSourceName NVARCHAR(128)
	DECLARE @activityName NVARCHAR(128)
	DECLARE @maxVersion INT
	DECLARE @nextVersion INT

	DECLARE eventsource_cursor CURSOR FOR
	SELECT EventSourceName, ActivityName, MAX(Version) FROM deleted GROUP BY EventSourceName, ActivityName

	OPEN eventsource_cursor
	FETCH NEXT FROM eventsource_cursor INTO @eventSourceName, @activityName, @maxVersion

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Check if event source needs to be removed from EventSource tbl 
		IF EXISTS(SELECT TOP 1 * FROM [dbo].[bam_Metadata_InterceptorConfiguration] WITH (SERIALIZABLE) WHERE
					  EventSourceName = @eventSourceName)
		BEGIN	
			IF EXISTS(SELECT TOP 1 * FROM [dbo].[bam_Metadata_InterceptorConfiguration] WHERE
					  EventSourceName = @eventSourceName AND ActivityName <> @activityName AND Version = @maxVersion)
			BEGIN
				-- Bump versions for any event source that was affected
				SET @nextVersion = @maxVersion + 1

				INSERT [dbo].[bam_Metadata_InterceptorConfiguration]
				SELECT EventSourceName, ActivityName, @nextVersion, Operator, TimeCreated, OnEventXml 
				FROM [dbo].[bam_Metadata_InterceptorConfiguration]
				WHERE EventSourceName = @eventSourceName 
				AND ActivityName <> @activityName
				AND Version = @maxVersion
			END
		END
		FETCH NEXT FROM eventsource_cursor INTO @eventSourceName, @activityName, @maxVersion
	END
	
	CLOSE eventsource_cursor
	DEALLOCATE eventsource_cursor