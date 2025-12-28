CREATE PROCEDURE [dbo].[trk_GetPit]
AS
BEGIN
	SELECT BamDBServerName, BamDBName FROM adm_Group
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_GetPit] TO [BTS_ADMIN_USERS]
    AS [dbo];

