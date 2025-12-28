CREATE PROCEDURE [dbo].[bam_Metadata_GetRTAWindowForCubeRta]
(
    @cubeName       sysname,    
    @rtaName        sysname
)
AS
    SELECT [rta].[RTAWindow]
    FROM [bam_Metadata_RealTimeAggregations] [rta]
    WHERE [rta].[CubeName] = @cubeName AND [rta].[RtaName] = @rtaName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRTAWindowForCubeRta] TO [BAM_ManagementWS]
    AS [dbo];

