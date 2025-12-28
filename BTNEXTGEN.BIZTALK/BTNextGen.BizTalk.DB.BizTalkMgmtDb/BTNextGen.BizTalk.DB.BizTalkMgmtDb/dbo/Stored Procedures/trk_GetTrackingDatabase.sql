CREATE PROCEDURE [dbo].[trk_GetTrackingDatabase] 
(
	@ModuleName nvarchar(256)
)
AS
BEGIN
	SELECT TrackingDBServerName,TrackingDBName,TrackAnalysisServerName,TrackAnalysisDBName
	FROM adm_Group 
	JOIN bts_assembly ON bts_assembly.nGroupId=adm_Group.Id
	WHERE nvcFullName=@ModuleName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_GetTrackingDatabase] TO [BTS_ADMIN_USERS]
    AS [dbo];

