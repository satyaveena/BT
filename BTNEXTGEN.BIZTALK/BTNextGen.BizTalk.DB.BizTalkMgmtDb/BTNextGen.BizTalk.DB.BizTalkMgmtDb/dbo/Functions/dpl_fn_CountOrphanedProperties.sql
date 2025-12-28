CREATE FUNCTION dpl_fn_CountOrphanedProperties()
RETURNS int

AS
BEGIN
  DECLARE @orphancount int
  SELECT @orphancount = COUNT(name) 
   FROM bt_Properties INNER JOIN bt_DocumentSpec ON bt_Properties.propSchemaID = bt_DocumentSpec.id
          INNER JOIN bt_XMLShare ON bt_DocumentSpec.shareid = bt_XMLShare.id
   WHERE NOT EXISTS ( SELECT ds.shareid 
      FROM bt_DocumentSpec ds
       INNER JOIN bt_XMLShare ON ds.shareid = bt_XMLShare.id 
      WHERE bt_XMLShare.active = 1 AND
       ds.msgtype = bt_Properties.namespace + N'#' + bt_Properties.name
     )
      AND
       bt_XMLShare.active = 1
  RETURN @orphancount
END