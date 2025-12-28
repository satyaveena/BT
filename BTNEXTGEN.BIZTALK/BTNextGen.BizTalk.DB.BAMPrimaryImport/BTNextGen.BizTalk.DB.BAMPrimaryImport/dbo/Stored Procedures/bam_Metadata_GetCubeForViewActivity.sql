CREATE PROCEDURE [dbo].[bam_Metadata_GetCubeForViewActivity]
(
    @viewName       sysname,
    @activityName   sysname    
)
AS
    SELECT [Cubes].[CubeName]
    FROM bam_Metadata_Cubes [Cubes]
    WHERE ViewName = @viewName AND ActivityName = @activityName