CREATE PROCEDURE [dbo].[bt_GetDocSpecInfoByName]
@name nvarchar(256),
@iPipelineAssemblyId int    -- also the assembly that implements the schema
AS
 SELECT d.id, d.msgtype, d.docspec_name, d.clr_assemblyname
 FROM           bt_DocumentSpec d WITH (ROWLOCK) 
     INNER JOIN bt_XMLShare x WITH (ROWLOCK) ON x.id = d.shareid
 WHERE    d.docspec_name = @name AND d.assemblyid = @iPipelineAssemblyId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_GetDocSpecInfoByName] TO [BTS_HOST_USERS]
    AS [dbo];

