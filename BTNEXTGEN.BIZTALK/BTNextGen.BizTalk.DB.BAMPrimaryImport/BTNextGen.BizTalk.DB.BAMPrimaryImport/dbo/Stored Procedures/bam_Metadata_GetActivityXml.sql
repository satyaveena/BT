CREATE PROCEDURE [dbo].[bam_Metadata_GetActivityXml]
(
    @viewName NVARCHAR(128) = NULL
)
AS
    IF (@viewName IS NULL)
        SELECT  ActivityName, 
                DefinitionXml
        FROM    [dbo].[bam_Metadata_Activities]
    ELSE
        SELECT  activities.ActivityName, 
                activities.DefinitionXml
        FROM    [dbo].[bam_Metadata_Activities] activities
        JOIN    [dbo].[bam_Metadata_ActivityViews] activityViews ON activityViews.ActivityName = activities.ActivityName
        JOIN    [dbo].[bam_Metadata_Views] views ON activityViews.ViewName = views.ViewName
        WHERE   views.ViewName = @viewName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetActivityXml] TO [BAM_ManagementWS]
    AS [dbo];

