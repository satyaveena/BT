CREATE PROCEDURE [dbo].[adm_ServiceInfo_Lookup]
@Name nvarchar(256),
@AssemblyName nvarchar(256),
@AssemblyVersion nvarchar(256),
@AssemblyCulture nvarchar(256),
@AssemblyPKToken nvarchar(256)
AS

 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 select
  svcCls.UniqueId,
  orch.uidGUID,
  asm.nvcFullName,
  grp.Name
 from
  bts_assembly asm,
  bts_orchestration orch,
  adm_ServiceClass svcCls,
  adm_Group grp
 where
  orch.nAssemblyID = asm.nID AND
  orch.nvcFullName = @Name AND
  asm.nvcName = @AssemblyName AND
  asm.nvcVersion = @AssemblyVersion AND
  asm.nvcCulture = @AssemblyCulture AND
  asm.nvcPublicKeyToken = @AssemblyPKToken AND
  svcCls.Name = N'XLANG/s'

 -- Give more specific error code.  If no record is found, then the service has not
 -- been deployed and/or enlisted.  If more than one record are found, this cannot
 -- happen.  But just return the regular error code in that case.
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ServiceInfo_Lookup] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ServiceInfo_Lookup] TO [BTS_OPERATORS]
    AS [dbo];

