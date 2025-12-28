CREATE PROCEDURE [dbo].[adm_Adapter_Create]
@Name nvarchar(256),
@Capabilities int,
@Comment nvarchar(256),
@MgmtCLSID nvarchar(256),
@InboundEngineCLSID nvarchar(256),
@InboundAssemblyPath nvarchar(256),
@InboundTypeName nvarchar(256),
@OutboundEngineCLSID nvarchar(256),
@OutboundAssemblyPath nvarchar(256),
@OutboundTypeName nvarchar(256),
@PropertyNameSpace nvarchar(256),
@AliasesXML nvarchar(2048),
@DefaultRHCfg ntext,
@DefaultTHCfg ntext
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 declare @bHasOpenedTransaction as int
 set @bHasOpenedTransaction = 0

 if(@@trancount = 0) -- this sp could be called from DTC
 begin
  begin transaction
  set @bHasOpenedTransaction = 1
 end
 
  if (N'' = @InboundEngineCLSID)
   set @InboundEngineCLSID = NULL
   
  if (N'' = @OutboundEngineCLSID)
   set @OutboundEngineCLSID = NULL

  -- If not NULL, InboundEngineCLSID must be unique
  if exists (
   select * from adm_Adapter
   where InboundEngineCLSID = @InboundEngineCLSID
  )
  begin
   set @ErrCode = 0xC0C025EB -- CIS_E_ADMIN_INBOUND_ENGINE_CLSID_CONFLICT
   goto exit_proc
  end

  -- If not NULL, OutboundEngineCLSID must be unique
  if exists (
   select * from adm_Adapter
   where OutboundEngineCLSID = @OutboundEngineCLSID
  )
  begin
   set @ErrCode = 0xC0C025EC -- CIS_E_ADMIN_OUTBOUND_ENGINE_CLSID_CONFLICT
   goto exit_proc
  end

  insert into adm_Adapter
  (
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
   DefaultRHCfg,
   DefaultTHCfg
  )
  values
  (
   @Name,
   @Capabilities,
   @Comment,
   @MgmtCLSID,
   @InboundEngineCLSID,
   @InboundAssemblyPath,
   @InboundTypeName,
   @OutboundEngineCLSID,
   @OutboundAssemblyPath,
   @OutboundTypeName,
   @PropertyNameSpace,
   @DefaultRHCfg,
   @DefaultTHCfg
  )
  
  declare @AdapterId as int
  set @AdapterId = @@Identity

  -- create aliases for the adapter  
  -- @AliasesXML is something like: '<ROOT><AdapterAlias alias="HTTP://"/>=<AdapterAlias alias="HTTPS://"/></ROOT>'
  -- or <AdapterAliasList><AdapterAlias>HTTP://</AdapterAlias><AdapterAlias>HTTPS://</AdapterAlias></AdapterAliasList>
  
  DECLARE @hDoc int
  EXEC sp_xml_preparedocument @hDoc OUTPUT, @AliasesXML 

  declare curse cursor for
  SELECT [text]
  FROM OPENXML(@hDoc, N'/') WHERE [text] is not null

  OPEN curse
  declare @value nvarchar(1024)

  FETCH NEXT FROM curse INTO @value
  WHILE (@@FETCH_STATUS = 0 AND @ErrCode = 0)
  BEGIN
   -- check if alias is already present
   if exists (
    select * from adm_AdapterAlias
    where AliasValue = @value
   )
   begin
    set @ErrCode = 0xC0C02814 -- CIS_E_ADMIN_CANNOT_ADD_PROTOCOL_BECAUSE_DUPLICATE_ALIASVALUE
    goto exit_proc
   end

   insert into adm_AdapterAlias
    (AdapterId, AliasValue) values (@AdapterId, @value)
   set @ErrCode = @@ERROR

   FETCH NEXT FROM curse INTO @value
  END
   
  CLOSE curse
  DEALLOCATE curse

  EXEC sp_xml_removedocument @hDoc
  if ( @ErrCode <> 0 ) goto exit_proc

 
exit_proc:

 if(0 <> @bHasOpenedTransaction)
 begin
  if(@ErrCode = 0)
   commit transaction
  else
  begin
   rollback transaction
  end
 end

 return @ErrCode
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Adapter_Create] TO [BTS_ADMIN_USERS]
    AS [dbo];

