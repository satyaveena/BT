CREATE PROCEDURE [dbo].[bam_Metadata_GetCubes]
(
    @viewName sysname
)
AS
    SELECT cubes.CubeName 
    FROM dbo.bam_Metadata_Cubes cubes
    WHERE cubes.ViewName = @viewName AND CreateOlapCube = 1