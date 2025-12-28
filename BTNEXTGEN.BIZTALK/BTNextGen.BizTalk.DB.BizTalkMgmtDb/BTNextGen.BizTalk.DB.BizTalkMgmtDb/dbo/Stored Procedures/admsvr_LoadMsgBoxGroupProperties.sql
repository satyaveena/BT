CREATE PROCEDURE [dbo].[admsvr_LoadMsgBoxGroupProperties]
@nvcGroupName nvarchar(256),
@nvcMasterDBServer nvarchar(80) OUTPUT,
@nvcMasterDBName nvarchar(128) OUTPUT,
@nNumMsgboxServers int OUTPUT,
@nCacheRefreshInterval int OUTPUT
AS
	set nocount on
	
	select TOP 1
		@nvcMasterDBServer = adm_Group.SubscriptionDBServerName,
		@nvcMasterDBName = adm_Group.SubscriptionDBName,
		@nCacheRefreshInterval = adm_Group.ConfigurationCacheRefreshInterval
	from
		adm_Group	
	select  
		adm_MessageBox.DBServerName, 
		adm_MessageBox.DBName, 
		adm_MessageBox.UniqueId,
		adm_MessageBox.DisableNewMsgPublication,
		adm_MessageBox.IsMasterMsgBox
	from
		adm_MessageBox WITH (REPEATABLEREAD),
		adm_Group 
	where	
		adm_MessageBox.GroupId = adm_Group.Id 	
	set @nNumMsgboxServers = @@ROWCOUNT
	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_LoadMsgBoxGroupProperties] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_LoadMsgBoxGroupProperties] TO [BTS_OPERATORS]
    AS [dbo];

