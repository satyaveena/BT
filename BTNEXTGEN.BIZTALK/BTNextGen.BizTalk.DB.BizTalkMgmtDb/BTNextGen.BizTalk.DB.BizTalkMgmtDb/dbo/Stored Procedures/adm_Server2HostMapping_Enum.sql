CREATE PROCEDURE [dbo].[adm_Server2HostMapping_Enum]
AS
 set nocount on
 set xact_abort on


 select
  adm_Server2HostMapping.Id,
  adm_Server.Name,
  adm_Host.Name,
  adm_Server2HostMapping.IsMapped,
  adm_Server2HostMapping.DateModified
 from
  adm_Server2HostMapping,
  adm_Server,
  adm_Host
 where
  adm_Server2HostMapping.ServerId = adm_Server.Id AND
  adm_Server2HostMapping.HostId = adm_Host.Id

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server2HostMapping_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server2HostMapping_Enum] TO [BTS_OPERATORS]
    AS [dbo];

