CREATE PROCEDURE [dbo].[trk_RemoveTrackingProfile]
	@ModuleName nvarchar(256),
	@Version nvarchar(20)
AS
BEGIN
	-- Remove all Interceptor Configurations
	DELETE from StaticTrackingInfo where exists
	(SELECT * FROM bts_orchestration
		 JOIN bts_assembly 
			ON bts_orchestration.nAssemblyID=bts_assembly.nID
		 WHERE StaticTrackingInfo.uidServiceId=uidGUID
		 AND uidInterceptorId='{58E2AB42-51EB-441D-9C93-8795982336B8}'
		 AND bts_assembly.nvcFullName=@ModuleName AND bts_assembly.nvcVersion=@Version)
	-- Remove the Tracking Profile binary blob
	UPDATE bts_assembly
	SET imgTrackingProfile = NULL
	WHERE nvcFullName=@ModuleName AND nvcVersion=@Version
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_RemoveTrackingProfile] TO [BTS_ADMIN_USERS]
    AS [dbo];

