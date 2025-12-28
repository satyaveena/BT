CREATE PROCEDURE [dbo].[dpl_SaveMap] 
(
 @ArtifactId int,
 @AssemblyId int,
 @IndocDocSpecName nvarchar (256) ,
 @OutdocDocSpecName nvarchar (256) ,
 @ArtifactXml ntext
)

AS
 if not (exists(select * from bt_DocumentSpec where docspec_name = @IndocDocSpecName) and
  exists(select * from bt_DocumentSpec where docspec_name = @OutdocDocSpecName))
  return -1 --Fail if in and out schemas of a map are not present

 DECLARE @shareid  uniqueidentifier
 SELECT @shareid = newid()
 INSERT INTO bt_XMLShare( id,target_namespace, active, content )
   VALUES( @shareid, N'', 1, @ArtifactXml )
 INSERT INTO bt_MapSpec( 
     itemid, 
     assemblyid,
     shareid,
     indoc_docspec_name,
     outdoc_docspec_name
    )
  VALUES( @ArtifactId, @AssemblyId, @shareid, @IndocDocSpecName, @OutdocDocSpecName )
 RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_SaveMap] TO [BTS_ADMIN_USERS]
    AS [dbo];

