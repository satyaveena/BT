CREATE PROCEDURE [dbo].[trk_HatGetHostList]
AS
	set nocount on
	set xact_abort on
	select
		Name
	from
		adm_Host
	set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_HatGetHostList] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_HatGetHostList] TO [BTS_OPERATORS]
    AS [dbo];

