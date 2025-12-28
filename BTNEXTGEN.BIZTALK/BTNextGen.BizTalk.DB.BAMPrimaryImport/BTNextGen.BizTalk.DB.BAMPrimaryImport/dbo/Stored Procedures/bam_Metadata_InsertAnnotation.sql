CREATE PROCEDURE [dbo].[bam_Metadata_InsertAnnotation]
(
    @ActivityName sysname,
    @Version uniqueidentifier,
    @MinorVersion int,
    @Subject nvarchar(256),
    @Component nvarchar(256),
    @TrackPointId int,
    @AnnotationXml ntext
)
AS
    INSERT INTO bam_Metadata_Annotations
    VALUES (
        @ActivityName,
        @Version,
        @MinorVersion,
        @Subject,
        @Component,
        @TrackPointId,
        @AnnotationXml
    )