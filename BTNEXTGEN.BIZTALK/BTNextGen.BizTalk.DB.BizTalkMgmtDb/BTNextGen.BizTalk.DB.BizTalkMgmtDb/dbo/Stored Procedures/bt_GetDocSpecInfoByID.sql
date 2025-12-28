CREATE PROCEDURE [dbo].[bt_GetDocSpecInfoByID]
@id uniqueidentifier
AS
 set nocount on
 
 SELECT d.id, d.msgtype, d.docspec_name, d.clr_assemblyname 
 FROM    bt_DocumentSpec d WITH (ROWLOCK) 
 WHERE    d.id = @id
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_GetDocSpecInfoByID] TO [BTS_HOST_USERS]
    AS [dbo];

