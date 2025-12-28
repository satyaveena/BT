CREATE PROCEDURE [dbo].[bam_RemoveTrackingProfile]
	@uidVersionId uniqueidentifier
AS
BEGIN
	DECLARE @idx int
	SELECT @idx = nID FROM bam_TrackingProfiles WHERE uidVersionId = @uidVersionId
	IF (@idx IS NOT NULL)
	BEGIN
		DELETE FROM bam_TrackPoints WHERE nProfileId = @idx
		DELETE FROM bam_TrackingProfiles WHERE uidVersionId = @uidVersionId
	END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_RemoveTrackingProfile] TO [BTS_ADMIN_USERS]
    AS [dbo];

