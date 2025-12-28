CREATE PROCEDURE [dbo].[bam_Metadata_GetActivityNamesForView]
(
    @viewName NVARCHAR(128)
)
AS
    SELECT  ActivityName
    FROM    [dbo].[bam_Metadata_ActivityViews]
    WHERE   ViewName = @viewName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetActivityNamesForView] TO [BAM_ManagementWS]
    AS [dbo];

