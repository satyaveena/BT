CREATE PROCEDURE [dbo].[adm_Adapter_Update]
@Name nvarchar(256),     -- ignored during update. This property is not updatable
@Capabilities int,      -- ignored during update. This property is not updatable
@Comment nvarchar(256),
@MgmtCLSID nvarchar(256),    -- ignored during update. This property is not updatable
@InboundEngineCLSID nvarchar(256),  -- ignored during update. This property is not updatable
@InboundAssemblyPath nvarchar(256),  -- ignored during update. This property is not updatable
@InboundTypeName nvarchar(256),   -- ignored during update. This property is not updatable
@OutboundEngineCLSID nvarchar(256),  -- ignored during update. This property is not updatable
@OutboundAssemblyPath nvarchar(256), -- ignored during update. This property is not updatable
@OutboundTypeName nvarchar(256),  -- ignored during update. This property is not updatable
@PropertyNameSpace nvarchar(256),
@AliasesXML nvarchar(2048),    -- ignored during update. This property is not updatable
@DefaultRHCfg ntext,     -- ignored during update. This property is not updatable
@DefaultTHCfg ntext      -- ignored during update. This property is not updatable
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction

  update adm_Adapter
  set
   Comment=@Comment,
   DateModified = GETUTCDATE()
  where
   Name = @Name

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback transaction
  return @ErrCode
 end

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Adapter_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

