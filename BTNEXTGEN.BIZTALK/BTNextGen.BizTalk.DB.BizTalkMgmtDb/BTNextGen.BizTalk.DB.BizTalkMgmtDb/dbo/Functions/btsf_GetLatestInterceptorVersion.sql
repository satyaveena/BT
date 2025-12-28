CREATE FUNCTION [dbo].[btsf_GetLatestInterceptorVersion] (@uidRootInterceptorID uniqueidentifier)
RETURNS uniqueidentifier
AS
BEGIN
	RETURN
		(SELECT TOP 1 uidInterceptorID
		FROM TrackinginterceptorVersions
		WHERE @uidRootInterceptorID = uidRootInterceptorID
		ORDER BY dtDeploymentTime DESC)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btsf_GetLatestInterceptorVersion] TO [BTS_HOST_USERS]
    AS [dbo];

