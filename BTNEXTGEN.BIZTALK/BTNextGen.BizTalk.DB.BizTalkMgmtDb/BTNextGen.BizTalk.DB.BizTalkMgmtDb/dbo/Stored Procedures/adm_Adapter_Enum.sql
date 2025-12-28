CREATE PROCEDURE [dbo].[adm_Adapter_Enum]
AS
 set nocount on
 set xact_abort on

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
 order by
  adm_Adapter.Name

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Adapter_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Adapter_Enum] TO [BTS_OPERATORS]
    AS [dbo];

