CREATE PROCEDURE [dbo].[admdta_GetTrackingProperties]
AS
	-- Get the tracking database with the group name first, this is for schedule services
	-- Then get all the tracking databases for pipeline deployment or deployment to an org
	SELECT	TrackingDBServerName,
			TrackingDBName,
			TrackingConfiguration
	FROM	adm_Group

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_GetTrackingProperties] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_GetTrackingProperties] TO [BTS_OPERATORS]
    AS [dbo];

