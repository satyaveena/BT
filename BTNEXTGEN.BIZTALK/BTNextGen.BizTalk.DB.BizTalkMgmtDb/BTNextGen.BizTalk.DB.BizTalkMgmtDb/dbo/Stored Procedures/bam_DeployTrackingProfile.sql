CREATE PROCEDURE [dbo].[bam_DeployTrackingProfile]
	@nvcName nvarchar(128),
	@uidVersionId uniqueidentifier,
	@nMinorVersionId int,
	@fForce bit
AS
BEGIN
	IF (@fForce = 1)
		EXEC bam_RemoveTrackingProfile @uidVersionId
	INSERT INTO bam_TrackingProfiles (nvcName, uidVersionId, nMinorVersionId) VALUES (@nvcName, @uidVersionId, @nMinorVersionId)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_DeployTrackingProfile] TO [BTS_ADMIN_USERS]
    AS [dbo];

