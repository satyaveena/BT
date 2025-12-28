CREATE FUNCTION [dbo].[adm_GetNumTransportConflictsInOrg] ()
RETURNS int
AS
BEGIN
 return
 (
  select count(*)
  from
   adm_Adapter,
   adm_Server,
   adm_Server2HostMapping as Map1,
   adm_Server2HostMapping as Map2,
   adm_Host as Host1,
   adm_Host as Host2,
   adm_ReceiveHandler as RcvHandler1,
   adm_ReceiveHandler as RcvHandler2
  where
   -- adapter has constraint
   (adm_Adapter.Capabilities & 4) <> 0-- eProtocolRequireSingleInstancePerServer
   -- instance of Host1 exists on a Server
   AND Map1.HostId = Host1.Id
   AND Map1.ServerId = adm_Server.Id
   AND Map1.IsMapped <> 0
   -- receive handler exists for the Host1
   AND RcvHandler1.HostId = Host1.Id
   AND RcvHandler1.AdapterId = adm_Adapter.Id
   -- instance of Host2 exists on a Server
   AND Map2.HostId = Host2.Id
   AND Map2.ServerId = adm_Server.Id
   AND Map2.IsMapped <> 0
   -- receive handler exists for the Host2
   AND RcvHandler2.HostId = Host2.Id
   AND RcvHandler2.AdapterId = adm_Adapter.Id
   -- hosts are different
   AND Host1.Id <> Host2.Id
 ) + (
  select count(*)
  from
   adm_Adapter,
   adm_Server,
   adm_Server2HostMapping as Map1,
   adm_Server2HostMapping as Map2,
   adm_Host as Host1,
   adm_Host as Host2,
   adm_SendHandler as SendHandler1,
   adm_SendHandler as SendHandler2
  where
   -- adapter has constraint
   (adm_Adapter.Capabilities & 4) <> 0-- eProtocolRequireSingleInstancePerServer
   -- instance of Host1 exists on a Server
   AND Map1.HostId = Host1.Id
   AND Map1.ServerId = adm_Server.Id
   AND Map1.IsMapped <> 0
   -- receive handler exists for the Host1
   AND SendHandler1.HostId = Host1.Id
   AND SendHandler1.AdapterId = adm_Adapter.Id
   -- instance of Host2 exists on a Server
   AND Map2.HostId = Host2.Id
   AND Map2.ServerId = adm_Server.Id
   AND Map2.IsMapped <> 0
   -- receive handler exists for the Host2
   AND SendHandler2.HostId = Host2.Id
   AND SendHandler2.AdapterId = adm_Adapter.Id
   -- hosts are different
   AND Host1.Id <> Host2.Id
 )
END