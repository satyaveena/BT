CREATE PROCEDURE [dbo].[trk_UpdateTrackingProfile] 
(
	@ModuleName nvarchar(256),
	@imgTrackingProfile image
)
AS
BEGIN
	UPDATE bts_assembly
	SET imgTrackingProfile = @imgTrackingProfile
	WHERE nvcFullName = @ModuleName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_UpdateTrackingProfile] TO [BTS_ADMIN_USERS]
    AS [dbo];

