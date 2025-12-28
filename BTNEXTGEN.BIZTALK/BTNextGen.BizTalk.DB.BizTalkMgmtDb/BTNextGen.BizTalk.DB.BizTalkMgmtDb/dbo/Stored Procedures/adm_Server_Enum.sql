CREATE PROCEDURE [dbo].[adm_Server_Enum]
AS
 set nocount on
 set xact_abort on

 select
  adm_Server.Id, 
  adm_Server.Name, 
  adm_Server.DateModified
 from
  adm_Server

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server_Enum] TO [BTS_OPERATORS]
    AS [dbo];

