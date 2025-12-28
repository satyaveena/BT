CREATE PROCEDURE [dbo].[bam_GetOrchestrationsForActivity]
@activityName nvarchar(128)
AS
	SELECT	serviceId
	FROM	[dbo].[bam_ActivityToOrchestrationMapping]
	WHERE	activityName = @activityName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_GetOrchestrationsForActivity] TO [BTS_ADMIN_USERS]
    AS [dbo];

