CREATE PROCEDURE [dbo].[adm_SendPort_Enum]
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 select
  sp.nID,
  sp.nvcName,
  spt.bIsPrimary,
  adapter.Name,
  spt.nvcAddress,
  host.Name,
  N'', -- No comment, to be removed
  spt.nvcTransportTypeData, -- CustomCfg
  pipeline.FullyQualifiedName,
  sp.DateModified
 from
  bts_sendport sp
 inner join bts_sendport_transport spt on spt.nSendPortID = sp.nID
 inner join adm_Adapter adapter on adapter.Id = spt.nTransportTypeId
 left join adm_Host host on host.Id = sp.nApplicationTypeId
 inner join bts_pipeline pipeline on pipeline.Id = sp.nSendPipelineID

 set nocount off
 return @ErrCode