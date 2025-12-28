-- Remove all versions of interceptor configuration
CREATE PROCEDURE [dbo].[bam_Metadata_RemoveInterceptorConfiguration]
(
	@activityName NVARCHAR(128),
	@eventSourceName NVARCHAR(128)=NULL
)
AS
	IF (@eventSourceName IS NULL)
	BEGIN
		DELETE FROM [dbo].[bam_Metadata_InterceptorConfiguration] WHERE ActivityName = @activityName
	END
	ELSE
	BEGIN		
		DELETE FROM [dbo].[bam_Metadata_InterceptorConfiguration] WHERE ActivityName = @activityName AND
		EventSourceName = @eventSourceName
	END