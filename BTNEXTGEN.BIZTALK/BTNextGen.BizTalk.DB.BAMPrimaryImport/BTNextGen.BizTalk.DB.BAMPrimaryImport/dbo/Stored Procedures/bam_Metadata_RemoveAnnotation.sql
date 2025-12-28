CREATE PROCEDURE [dbo].[bam_Metadata_RemoveAnnotation]
(
    @VersionId uniqueidentifier
)
AS
    DELETE 
    FROM    bam_Metadata_Annotations
    WHERE    Version = @VersionId