CREATE PROCEDURE [dbo].[adm_SendPort_Load]
@Name nvarchar(256),
@PrimaryTransport int
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 select @ErrCode = 0
 
  --case (select count(*) from adm_DefaultReceiveHandler where adm_DefaultReceiveHandler.ReceveiHandlerId = adm_ReceiveHandler.Id)
  -- when 0 then 0 else -1 end as DefaultHandler,

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
 where 
  sp.nvcName = @Name and
  spt.bIsPrimary = @PrimaryTransport
 
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendPort_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendPort_Load] TO [BTS_OPERATORS]
    AS [dbo];

