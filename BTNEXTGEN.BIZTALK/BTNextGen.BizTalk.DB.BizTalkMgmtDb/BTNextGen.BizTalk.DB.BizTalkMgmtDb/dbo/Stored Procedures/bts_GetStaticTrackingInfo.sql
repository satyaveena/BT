CREATE PROCEDURE [dbo].[bts_GetStaticTrackingInfo]
@uidServiceID uniqueidentifier,
@uidInterceptorID uniqueidentifier
AS
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
SET NOCOUNT ON
SELECT	d.dtDeploymentTime,
		d.imgData
FROM 	StaticTrackingInfo d
INNER JOIN adm_Group g ON g.GlobalTrackingOption <> 0
INNER JOIN TrackinginterceptorVersions tv ON tv.uidRootInterceptorID = @uidInterceptorID
WHERE 	d.uidServiceId = @uidServiceID AND 
		d.uidInterceptorId = tv.uidInterceptorID
ORDER BY d.dtDeploymentTime DESC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetStaticTrackingInfo] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetStaticTrackingInfo] TO [BTS_OPERATORS]
    AS [dbo];

