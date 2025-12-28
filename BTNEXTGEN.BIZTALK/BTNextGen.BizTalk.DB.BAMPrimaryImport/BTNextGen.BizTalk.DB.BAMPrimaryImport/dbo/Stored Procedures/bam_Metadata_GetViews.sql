CREATE PROCEDURE [dbo].[bam_Metadata_GetViews]
(
    @activityName sysname
)
AS
    SELECT     ViewName = bamViews.ViewName
    FROM [dbo].[bam_Metadata_Views] bamViews
        LEFT JOIN [dbo].[bam_Metadata_ActivityViews] bamActivityViews ON bamActivityViews.ViewName = bamViews.ViewName
    WHERE bamActivityViews.ActivityName = @activityName
    ORDER BY bamViews.ViewName