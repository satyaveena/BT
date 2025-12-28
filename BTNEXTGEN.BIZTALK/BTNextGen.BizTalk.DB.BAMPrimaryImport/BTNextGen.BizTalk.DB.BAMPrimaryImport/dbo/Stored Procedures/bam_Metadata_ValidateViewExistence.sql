CREATE PROCEDURE [dbo].[bam_Metadata_ValidateViewExistence]
(
    @viewName sysname
)
AS
    SELECT ViewName FROM dbo.bam_Metadata_Views WHERE ViewName = @viewName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_ValidateViewExistence] TO [BAM_ManagementWS]
    AS [dbo];

