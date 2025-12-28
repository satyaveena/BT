CREATE procedure [dbo].[bt_LoadPropertiesForDocSpec]
@strongName nvarchar(1024)

AS
 set nocount on

 select p.id, 
  p.name, 
  p.namespace, 
  p.xpath, 
  d.xsd_type, 
  p.is_tracked
 from bt_Properties  p 
 inner join bt_DocumentSpec d on p.itemid = d.itemid -- join on item ids
 inner join bts_assembly a on a.nID = d.assemblyid
 where   d.schema_root_clr_fqn + ', ' + a.nvcFullName = @strongName 
  AND p.is_tracked <> 0 -- return only the tracked items 
  
 UNION
 
 select 
  docs.id,
  docs.schema_root_name, 
  shares.target_namespace,
  N'', -- xpath 
  docs.xsd_type, 
  docs.is_tracked
 FROM   dbo.bt_DocumentSpec docs
 INNER JOIN dbo.bt_XMLShare shares  ON docs.shareid = shares.id
 WHERE docs.is_property_schema <> 0
  AND docs.is_tracked <> 0 -- return only the tracked items

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_LoadPropertiesForDocSpec] TO [BTS_HOST_USERS]
    AS [dbo];

