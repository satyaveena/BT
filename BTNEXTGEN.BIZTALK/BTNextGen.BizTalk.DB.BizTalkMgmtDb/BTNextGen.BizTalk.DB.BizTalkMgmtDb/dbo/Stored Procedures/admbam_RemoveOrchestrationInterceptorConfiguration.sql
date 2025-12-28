CREATE PROCEDURE [dbo].[admbam_RemoveOrchestrationInterceptorConfiguration]
@uidServiceId uniqueidentifier,
@uidInterceptorId uniqueidentifier
AS
	DELETE FROM StaticTrackingInfo
	WHERE uidServiceId = @uidServiceId
	    AND uidInterceptorId = @uidInterceptorId

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admbam_RemoveOrchestrationInterceptorConfiguration] TO [BTS_ADMIN_USERS]
    AS [dbo];

