CREATE PROCEDURE [dbo].[bam_InsertActivityOrchestrationRelation]
@activityName nvarchar(128),
@serviceId uniqueIdentifier
AS
	INSERT INTO [dbo].[bam_ActivityToOrchestrationMapping]
	(
		activityName,
		serviceId
	)
	VALUES
	(
		@activityName,
		@serviceId
	)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_InsertActivityOrchestrationRelation] TO [BTS_ADMIN_USERS]
    AS [dbo];

