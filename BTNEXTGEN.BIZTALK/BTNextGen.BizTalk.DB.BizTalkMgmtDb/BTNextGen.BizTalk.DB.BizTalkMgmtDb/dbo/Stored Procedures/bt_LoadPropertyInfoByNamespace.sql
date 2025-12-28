CREATE PROCEDURE [dbo].[bt_LoadPropertyInfoByNamespace]
@nvcNamespace nvarchar(256)

AS

set nocount on
set transaction isolation level read committed

declare @exists int
--Check to see if this is the xsd namespace. If not, assume it is the clr namespace
select @exists = count(*) FROM bt_XMLShare WHERE target_namespace = @nvcNamespace AND active = 1
if (@exists > 0)
BEGIN
 SELECT d.id, d.msgtype, d.property_clr_class_fqn, d.xsd_type
 FROM bt_DocumentSpec d 
     INNER JOIN bt_XMLShare x ON d.shareid=x.id
     INNER JOIN bts_assembly a ON a.nID = d.assemblyid
 WHERE x.target_namespace = @nvcNamespace 
 ORDER BY a.nvcName ASC, a.nvcPublicKeyToken ASC, a.nvcCulture ASC, a.nVersionMajor ASC, a.nVersionMinor ASC, a.nVersionRevision ASC, a.nVersionBuild ASC
END
ELSE
BEGIN
 SELECT d.id, d.msgtype, d.property_clr_class_fqn, d.xsd_type
 FROM        bt_DocumentSpec d
        INNER JOIN bt_XMLShare x WITH (ROWLOCK) ON x.id = d.shareid
     INNER JOIN bts_assembly a ON a.nID = d.assemblyid
 WHERE d.clr_namespace = @nvcNamespace
 ORDER BY a.nvcName ASC, a.nvcPublicKeyToken ASC, a.nvcCulture ASC, a.nVersionMajor ASC, a.nVersionMinor ASC, a.nVersionRevision ASC, a.nVersionBuild ASC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_LoadPropertyInfoByNamespace] TO [BTS_HOST_USERS]
    AS [dbo];

