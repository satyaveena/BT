CREATE PROCEDURE [dbo].[bts_validate_started_sendportgroup]
@nSPGID int
AS
	--// validate started send port group, it must have at least one enlisted send port
	select count(*) from bts_spg_sendport spg_sp 
	join bts_sendport sp on sp.nID = spg_sp.nSendPortID and sp.nPortStatus > 1
	where spg_sp.nSendPortGroupID = @nSPGID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_started_sendportgroup] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_validate_started_sendportgroup] TO [BTS_OPERATORS]
    AS [dbo];

