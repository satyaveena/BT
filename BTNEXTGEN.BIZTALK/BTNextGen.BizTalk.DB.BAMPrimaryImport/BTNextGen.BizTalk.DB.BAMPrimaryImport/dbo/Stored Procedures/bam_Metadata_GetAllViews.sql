CREATE PROCEDURE [dbo].[bam_Metadata_GetAllViews] 
AS
    SELECT ViewName = bamViews.ViewName
    FROM [dbo].[bam_Metadata_Views] bamViews
    ORDER BY bamViews.ViewName