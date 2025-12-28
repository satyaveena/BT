CREATE PROCEDURE [dbo].[bam_Metadata_RetrieveTrackingProfiles]
    @ReferencedBy nvarchar(300)
AS
    SELECT    ActivityName, VersionId, ProfileXml
    FROM    [dbo].[bam_Metadata_TrackingProfiles]
    WHERE    ReferencedBy = @ReferencedBy