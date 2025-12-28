CREATE PROCEDURE [dbo].[trk_SetPit] 
(
	@ServerName nvarchar(80),
	@DatabaseName nvarchar(128)
)
AS
BEGIN
	UPDATE adm_Group
	SET BamDBServerName = @ServerName, BamDBName = @DatabaseName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_SetPit] TO [BTS_ADMIN_USERS]
    AS [dbo];

