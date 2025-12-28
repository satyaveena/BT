CREATE PROCEDURE [dbo].[trk_HatGetConfig]
AS
	set nocount on
	set xact_abort on
	select
		Name,
		TrackingDBServerName, 
		TrackingDBName, 
		TrackAnalysisServerName, 
		TrackAnalysisDBName 
	from
		adm_Group
	set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_HatGetConfig] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_HatGetConfig] TO [BTS_OPERATORS]
    AS [dbo];

