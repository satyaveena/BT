CREATE PROCEDURE [dbo].[bt_LoadDocSpecByName]
@name nvarchar(1024),
@nSame int OUTPUT

AS
 set nocount on

 set @nSame = 0
 if (@name = N'Unparsed Interchange')
  OR (@name = N'Serialized Interchange')
  OR (@name = N'http://schemas.microsoft.com/BizTalk/2003/Any#Root')
 begin
  select cast (N'{00000000-0000-0000-0000-000000000001}' as uniqueidentifier),
    getutcdate(),
    @name,
    @name,
    N'Body_XPath',
    cast (N'{00000000-0000-0000-0000-000000000001}' as uniqueidentifier),
    @name,
    @name,
    null -- xs.content
 end
 else
 begin
  select ds.id,
    ds.date_modified,
    ds.docspec_name,
    ds.msgtype,
    ds.body_xpath,
    xs.id,
    xs.target_namespace,
    @name,
    null -- xs.content
  from bt_DocumentSpec ds
  inner join bt_XMLShare xs on xs.id = ds.shareid
  inner join bts_assembly a on a.nID = ds.assemblyid
  where   ds.schema_root_clr_fqn + ', ' + a.nvcFullName = @name 
 end

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_LoadDocSpecByName] TO [BTS_HOST_USERS]
    AS [dbo];

