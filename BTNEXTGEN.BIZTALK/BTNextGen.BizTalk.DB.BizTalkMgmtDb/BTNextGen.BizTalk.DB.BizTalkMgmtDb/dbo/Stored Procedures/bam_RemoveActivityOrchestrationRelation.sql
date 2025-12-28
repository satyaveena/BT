CREATE PROCEDURE [dbo].[bam_RemoveActivityOrchestrationRelation]
@activityName nvarchar(128)
AS
	DELETE
	FROM	[dbo].[bam_ActivityToOrchestrationMapping]
	WHERE	activityName = @activityName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_RemoveActivityOrchestrationRelation] TO [BTS_ADMIN_USERS]
    AS [dbo];

