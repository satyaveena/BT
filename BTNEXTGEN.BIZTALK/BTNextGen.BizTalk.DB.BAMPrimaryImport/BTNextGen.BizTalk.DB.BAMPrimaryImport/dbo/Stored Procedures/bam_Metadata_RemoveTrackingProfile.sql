CREATE PROCEDURE [dbo].[bam_Metadata_RemoveTrackingProfile]
(
    @ActivityName nvarchar(128),
    @ReferencedBy nvarchar(300)
)
AS
    DELETE
    FROM [dbo].[bam_Metadata_TrackingProfiles]
    WHERE ReferencedBy = @ReferencedBy AND ActivityName = @ActivityName