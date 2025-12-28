CREATE PROCEDURE [dbo].[adm_IsBTSAdmin]
AS
 return 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_IsBTSAdmin] TO [BTS_ADMIN_USERS]
    AS [dbo];

