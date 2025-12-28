CREATE PROCEDURE [dbo].[bts_InsertStaticTrackingInfo]
@strServiceName nvarchar(256),
@uidServiceID uniqueidentifier,
@uidInterceptorID uniqueidentifier,
@dtTimeStamp datetime,
@ismsgBodyTrackingEnabled int,
@imgData image
AS
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	SET NOCOUNT ON
	-- Today HAT only passes the Root Interceptor Id here, so we do not know which version of this
	-- interceptor will be used. Because of that, currently we are always defaulting to the latest one.
	UPDATE	StaticTrackingInfo
	SET		ismsgBodyTrackingEnabled = @ismsgBodyTrackingEnabled,
			imgData = @imgData
	FROM	StaticTrackingInfo sti, TrackinginterceptorVersions tv
	WHERE	sti.uidServiceId = @uidServiceID 
			AND sti.uidInterceptorId = tv.uidInterceptorID
			AND tv.uidRootInterceptorID = @uidInterceptorID
	IF ( @@ROWCOUNT = 0 )
	BEGIN
		-- If the deployment time is null, then check if this artifact has been
		-- deployed before. If yes, then get the deployment time
		IF (@dtTimeStamp IS NULL)
		BEGIN
			SET @dtTimeStamp = (SELECT MAX(dtDeploymentTime)
				FROM 	StaticTrackingInfo
				WHERE	uidServiceId = @uidServiceID)
		END
	
		-- If there is no deployment time anywhere, then get the current time
		IF (@dtTimeStamp IS NULL)
		BEGIN
			SET @dtTimeStamp = GETUTCDATE()
		END
		INSERT INTO StaticTrackingInfo (
			strServiceName,
			uidServiceId,
			uidInterceptorId,
			dtDeploymentTime,
			ismsgBodyTrackingEnabled,
			imgData)
		VALUES (
			@strServiceName,
			@uidServiceID,
			dbo.btsf_GetLatestInterceptorVersion(@uidInterceptorID),
			@dtTimeStamp,
			@ismsgBodyTrackingEnabled,
			@imgData
			)
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_InsertStaticTrackingInfo] TO [BTS_HOST_USERS]
    AS [dbo];

