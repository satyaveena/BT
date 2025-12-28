CREATE PROCEDURE [dbo].[bts_InsertDefaultConfiguration]
@strServiceName		nvarchar(256),
@uidServiceID		uniqueidentifier,
@uidInterceptorID	uniqueidentifier,
@nMsgBodyTracking	int,
@imgConfiguration	image
AS
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	SET NOCOUNT ON
	declare	@dtTimeStamp datetime
	set @dtTimeStamp = GETUTCDATE()
	-- First try to see if this service was already deployed before
	-- If so, just update the deployment times
	--
	-- Today HAT only passes the Root Interceptor Id here, so we do not know which version of this
	-- interceptor will be used. Because of that, currently we are always defaulting to the latest one.
	UPDATE	StaticTrackingInfo
	SET 	dtDeploymentTime = @dtTimeStamp,
			dtUndeploymentTime = null,
			uidInterceptorId = dbo.btsf_GetLatestInterceptorVersion(@uidInterceptorID)
	FROM	StaticTrackingInfo sti, TrackinginterceptorVersions tv
	WHERE 	sti.uidServiceId = @uidServiceID 
			AND tv.uidRootInterceptorID = @uidInterceptorID
			AND sti.uidInterceptorId = tv.uidInterceptorID
	-- If this service was not there previously then insert a new one
	IF ( @@ROWCOUNT = 0 )
	BEGIN
		INSERT INTO StaticTrackingInfo 
		(
			strServiceName,
			uidServiceId,
			uidInterceptorId,
			dtDeploymentTime,
			ismsgBodyTrackingEnabled,
			imgData
		)
		VALUES
		(
			@strServiceName,
			@uidServiceID,
			dbo.btsf_GetLatestInterceptorVersion(@uidInterceptorID),
			@dtTimeStamp,
			@nMsgBodyTracking,
			@imgConfiguration
		)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_InsertDefaultConfiguration] TO [BTS_ADMIN_USERS]
    AS [dbo];

