CREATE PROCEDURE [dbo].[bam_Metadata_GetProperty]
(
    @propertyName            sysname,
    @scope                  sysname = N'Global'
)
AS
    SELECT PropertyValue FROM dbo.bam_Metadata_Properties 
    WHERE Scope = @scope AND PropertyName = @propertyName