CREATE PROCEDURE [dbo].[bts_GetInterceptors]
AS
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
SET NOCOUNT ON
SELECT	tiv.uidInterceptorID, 
		ti.InterceptorType, 
		tiv.AssemblyName, 
		tiv.TypeName
FROM	TrackinginterceptorVersions tiv
INNER JOIN Trackinginterceptor ti ON tiv.uidRootInterceptorID = ti.uidInterceptorID
WHERE	tiv.uidInterceptorID = (SELECT dbo.btsf_GetLatestInterceptorVersion(tiv.uidRootInterceptorID))

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetInterceptors] TO [BTS_HOST_USERS]
    AS [dbo];

