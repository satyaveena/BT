CREATE PROCEDURE [dbo].[adm_Adapter_Load]
@Name nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 select
  Id,
  Name,
  Capabilities,
  Comment,
  MgmtCLSID,
  InboundEngineCLSID,
  InboundAssemblyPath,
  InboundTypeName,
  OutboundEngineCLSID,
  OutboundAssemblyPath,
  OutboundTypeName,
  PropertyNameSpace,
  N'' as AliasesXML, -- this property is read only
  DateModified,
  DefaultRHCfg,
  DefaultTHCfg
 from
  adm_Adapter
 where
  Name = @Name

 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Adapter_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Adapter_Load] TO [BTS_OPERATORS]
    AS [dbo];

