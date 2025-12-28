CREATE PROCEDURE [dbo].[bt_LoadDocSpecByID]
@id uniqueidentifier,
@nUpdate int,
@mytime datetime,
@nSame int OUTPUT

AS
 set nocount on
 declare @dbtime datetime
 select @nSame = 0
 if ( (@nUpdate > 0) AND (@mytime > 0) ) 
 begin
  select top 1 @dbtime = d.date_modified
   from    bt_DocumentSpec d WITH (ROWLOCK)
   where   d.id = @id 
  if (DATEDIFF(second, @dbtime, @mytime) = 0)
  begin
     select @nSame = 1        
     return
  end     
 end


 DECLARE @sid uniqueidentifier
 SELECT @sid = xs.id 
  FROM bt_XMLShare xs 
  INNER JOIN bt_DocumentSpec ds ON ds.shareid = xs.id
  WHERE ds.id = @id 


 SELECT d.id, d.date_modified, d.docspec_name, d.msgtype doctype, d.body_xpath, x.id, 
  x.target_namespace, 
  d.schema_root_clr_fqn + ', ' + a.nvcFullName,
--  d.property_clr_class_fqn,
  NULL -- x.content 
 FROM    bt_DocumentSpec d WITH (ROWLOCK),
  bt_XMLShare x WITH (ROWLOCK),
  bts_assembly a WITH (ROWLOCK)
 WHERE    d.id = @id 
  AND d.assemblyid = a.nID
  AND ( x.id = d.shareid 
    OR 
    x.id IN ( SELECT id from bt_XMLShare xs1 WITH (ROWLOCK)
       WHERE xs1.target_namespace IN (
        SELECT target_namespace 
        FROM bt_XMLShareReferences xsr WITH (ROWLOCK)
        WHERE xsr.shareid = @sid 
       ) AND
      xs1.active = 1
    )
   )
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_LoadDocSpecByID] TO [BTS_HOST_USERS]
    AS [dbo];

