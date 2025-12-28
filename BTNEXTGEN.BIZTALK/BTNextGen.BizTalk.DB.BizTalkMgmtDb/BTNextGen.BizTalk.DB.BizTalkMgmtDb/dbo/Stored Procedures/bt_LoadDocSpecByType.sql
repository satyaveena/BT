CREATE procedure [dbo].[bt_LoadDocSpecByType]
@doctype nvarchar(256),
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
   from           bt_DocumentSpec d WITH (ROWLOCK)
          INNER JOIN bt_XMLShare x WITH (ROWLOCK) ON x.id = d.shareid
   where   d.msgtype = @doctype AND x.active = 1 

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
  WHERE ds.msgtype = @doctype AND xs.active = 1 


 SELECT d.id, d.date_modified, d.docspec_name, d.msgtype doctype, d.body_xpath, x.id, 
  x.target_namespace, d.property_clr_class_fqn, x.content 
 FROM    bt_DocumentSpec d WITH (ROWLOCK),
      bt_XMLShare x WITH (ROWLOCK)
 WHERE  d.msgtype = @doctype
   AND
   ( x.id = d.shareid 
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
      AND
     x.active = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_LoadDocSpecByType] TO [BTS_HOST_USERS]
    AS [dbo];

