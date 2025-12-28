CREATE PROCEDURE [dbo].[dpl_SaveDocType]
(
 @ArtifactId int,
 @ModuleId int,
 @Name nvarchar(2000),
 @uid uniqueidentifier = NULL,
 @MsgType nvarchar(2000),
 @isEnvelope bit,
 @isMultiroot bit,
 @isFlat bit,
 @body_xpath nvarchar(4000),
 @targetNamespace nvarchar(2000),
 @ArtifactXml ntext,
 @ImportList ntext,
 @isPropertySchema tinyint,
 @ArtifactClrNamespace nvarchar(256) = N'',
 @ArtifactClrTypeName nvarchar(256),
 @ArtifactAssemblyName nvarchar(512),
 @DocTypeElementName nvarchar(2000),
 @XsdType nvarchar(30),
 @PropertyCLRClass nvarchar(2000) = NULL,
 @ErrorCode int OUTPUT
)

AS
 DECLARE @shareid uniqueidentifier
 
 SELECT @ErrorCode = 0
 
    DECLARE @moduleName nvarchar(256)
 if (@uid IS NULL)
  set @uid = NewID()
 -- adjust ModuleId - 
 SELECT @ModuleId = AssemblyId FROM bts_item WHERE id = @ArtifactId
 SELECT @moduleName = nvcName
 FROM bts_assembly
 WHERE nID = @ModuleId
 
 DECLARE @isTracked int
 SELECT @isTracked = ds.is_tracked 
 FROM bt_DocumentSpec ds 
 INNER JOIN bt_XMLShare xs ON xs.id = ds.shareid
 INNER JOIN bts_assembly md ON ds.assemblyid = md.nID
 WHERE md.nvcName = @moduleName
  AND ds.clr_namespace = @ArtifactClrNamespace
  AND ds.clr_typename = @ArtifactClrTypeName
  AND xs.active = 1
 IF (@isTracked is null)
 BEGIN
  SET @isTracked = 0
 END
 -- Deactivate previous active schema version
 UPDATE bt_XMLShare 
  SET active = 0
  FROM bt_XMLShare x
  INNER JOIN bt_DocumentSpec ds
   INNER JOIN bts_assembly md
   ON md.nID = ds.assemblyid AND ds.msgtype = @MsgType AND md.nvcName = @moduleName
  ON x.id = ds.shareid
  WHERE x.active = 1

 SELECT TOP 1 @shareid = shareid 
  FROM bt_DocumentSpec
  WHERE @ArtifactId = itemid 
  
 IF ( @@ROWCOUNT = 0 )
  BEGIN  
   SELECT @shareid = newid()
   IF LEN( @targetNamespace ) = 0
    SELECT @targetNamespace = NULL
   INSERT INTO bt_XMLShare( id,
       target_namespace,  
       content, 
       active 
      )
      VALUES( @shareid, 
       @targetNamespace,  
       @ArtifactXml,
        0 -- doesn't matter here, will be overwritten below, need something NOT NULL
     )
   DECLARE @idoc int
   EXEC sp_xml_preparedocument @idoc OUTPUT, @ImportList
   INSERT INTO bt_XMLShareReferences
   SELECT DISTINCT  @shareid, targetNamespace
   FROM OPENXML (@idoc, N'/root/reference',2)
   WITH ( targetNamespace nvarchar(256) N'@targetNamespace' )
   
   EXEC sp_xml_removedocument @idoc
  END
 IF ( @isFlat = 0)
 BEGIN
  SELECT @ErrorCode = COUNT(ds.itemid) -- Check uniqueness of msgtype in DB (targetNamespace#rootelement needs to be unique)
      FROM  bt_DocumentSpec ds INNER JOIN bt_XMLShare xs ON ds.shareid = xs.id
      WHERE ds.msgtype = @MsgType 
  IF( @ErrorCode > 0 )
  BEGIN
   SELECT @ErrorCode = -1
  END
 END
 ELSE
 BEGIN
  SELECT @ErrorCode = COUNT(ds.itemid) -- Check uniqueness of docspec_name in DB (needs to be unique for flat files)
      FROM  bt_DocumentSpec ds INNER JOIN bt_XMLShare xs ON ds.shareid = xs.id
      WHERE ds.docspec_name = @ArtifactClrNamespace + N'.' + @ArtifactClrTypeName AND
        xs.active = 1
  IF( @ErrorCode > 0 )
  BEGIN
   SELECT @ErrorCode = -2
  END
 END
 DECLARE @deps AS int
 SELECT @deps = COUNT(*) 
  FROM bt_DocumentSpec 
  WHERE id = @uid AND msgtype <> @MsgType
 IF ( @deps > 0  )
 BEGIN
   SELECT @ErrorCode = -3
 END
 -- property schema supplies a unique identifier
 IF NOT EXISTS (SELECT ds.id 
     FROM bt_DocumentSpec ds
      INNER JOIN bts_item it ON ds.itemid = it.id
     WHERE it.SchemaType = 2 AND it.id = @ArtifactId )
 BEGIN
  INSERT INTO bt_DocumentSpec (
    id, 
    itemid,
    assemblyid,
    msgtype,
    shareid,
    body_xpath,
    is_property_schema,
    is_multiroot,
    clr_namespace,
    clr_typename,
    clr_assemblyname,
    schema_root_name,
    xsd_type,
    is_flat,
    property_clr_class,
    is_tracked
   )
  VALUES (
   @uid,
   @ArtifactId,
   @ModuleId,
   @MsgType,
   @shareid,
   @body_xpath,
   @isPropertySchema,
   @isMultiroot,
   @ArtifactClrNamespace,
   @ArtifactClrTypeName,
   @ArtifactAssemblyName,
   @DocTypeElementName,
   @XsdType,
   @isFlat,
   @PropertyCLRClass,
   @isTracked
  )
 END
DECLARE @ret int
SELECT @ret = @@IDENTITY
   
-- Activate schema coming from assembly with the highest version number
   
UPDATE bt_XMLShare 
 SET active = 1
 FROM bt_XMLShare x
 INNER JOIN bt_DocumentSpec ds
  INNER JOIN ( SELECT TOP 1 md.nID FROM bts_assembly md
    WHERE md.nvcName = @moduleName
    ORDER BY md.nVersionMajor DESC, md.nVersionMinor DESC, md.nVersionBuild DESC, md.nVersionRevision DESC
   ) as mdOuter
  ON mdOuter.nID = ds.assemblyid AND ds.msgtype = @MsgType
    ON x.id = ds.shareid
RETURN @ret
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_SaveDocType] TO [BTS_ADMIN_USERS]
    AS [dbo];

