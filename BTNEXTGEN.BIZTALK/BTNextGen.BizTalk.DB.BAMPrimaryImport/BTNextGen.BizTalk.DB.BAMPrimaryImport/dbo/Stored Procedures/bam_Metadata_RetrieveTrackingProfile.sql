CREATE PROCEDURE [dbo].[bam_Metadata_RetrieveTrackingProfile]
(
    @ActivityName nvarchar(128),
    @ReferencedBy nvarchar(300)
)
AS
    SELECT    VersionId, MinorVersionId, ProfileXml 
    FROM    [dbo].[bam_Metadata_TrackingProfiles]
    WHERE    ActivityName = @ActivityName 
        AND ReferencedBy = @ReferencedBy