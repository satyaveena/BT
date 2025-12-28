CREATE PROCEDURE [dbo].[bam_Metadata_RemoveProperty]
(
    @propertyName            sysname,
    @scope                  sysname = N'Global'
)
AS
    DELETE FROM dbo.bam_Metadata_Properties 
    WHERE Scope = @scope AND PropertyName = @propertyName