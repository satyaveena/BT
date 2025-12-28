CREATE PROCEDURE [dbo].[bam_Metadata_GetAnnotation]
(
    @Version uniqueidentifier,
    @MinorVersion int,
    @TrackPointId int
)
AS
    SELECT AnnotationXml
    FROM bam_Metadata_Annotations
    WHERE @Version = Version
        AND @MinorVersion = MinorVersion
        AND @TrackPointId = TrackPointId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAnnotation] TO [BAM_EVENT_WRITER]
    AS [dbo];

