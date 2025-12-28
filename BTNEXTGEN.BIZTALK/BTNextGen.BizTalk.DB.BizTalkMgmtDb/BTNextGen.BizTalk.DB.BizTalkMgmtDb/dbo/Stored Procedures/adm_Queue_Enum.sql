CREATE PROCEDURE [dbo].[adm_Queue_Enum]
AS
 select
  host.Name
 from
  adm_Host host
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Queue_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Queue_Enum] TO [BTS_OPERATORS]
    AS [dbo];

