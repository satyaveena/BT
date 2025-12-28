CREATE PROCEDURE [dbo].[bts_GetServiceStaticTrackingInfo]
@uidServiceID uniqueidentifier
AS
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
SET NOCOUNT ON
SELECT	d.dtDeploymentTime, 
		t.InterceptorType, 
		tv.AssemblyName, 
		tv.TypeName,
		d.imgData, 
		d.uidInterceptorId, 
		d.ismsgBodyTrackingEnabled
FROM	StaticTrackingInfo d
INNER JOIN TrackinginterceptorVersions tv ON d.uidInterceptorId = tv.uidInterceptorID
INNER JOIN Trackinginterceptor t ON tv.uidRootInterceptorID = t.uidInterceptorID 
INNER JOIN adm_Group g ON g.GlobalTrackingOption <> 0
WHERE	d.uidServiceId = @uidServiceID AND
		tv.uidRootInterceptorID = N'{1E83A7DC-435E-49DF-BA83-F09CA50DFBE7}' -- Health Monitoring Interceptor
UNION ALL
SELECT	d.dtDeploymentTime, 
		t.InterceptorType, 
		tv.AssemblyName, 
		tv.TypeName,
		d.imgData, 
		d.uidInterceptorId, 
		d.ismsgBodyTrackingEnabled
FROM	StaticTrackingInfo d
INNER JOIN TrackinginterceptorVersions tv ON d.uidInterceptorId = tv.uidInterceptorID
INNER JOIN Trackinginterceptor t ON tv.uidRootInterceptorID = t.uidInterceptorID 
WHERE	d.uidServiceId = @uidServiceID
		AND tv.uidRootInterceptorID <> N'{1E83A7DC-435E-49DF-BA83-F09CA50DFBE7}' -- All other interceptors
ORDER BY d.dtDeploymentTime DESC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetServiceStaticTrackingInfo] TO [BTS_HOST_USERS]
    AS [dbo];

