CREATE PROCEDURE [dbo].[dpl_SaveSchemaProperty]
(
 @ModuleId int,
 @ItemId int,
 @MsgType nvarchar(2000),
 @NameSpace nvarchar(2000),
 @KeyName nvarchar(2000),
 @XPath nvarchar(4000)
)

AS

DECLARE @propSchemaMsgtype nvarchar(4000)
SELECT @propSchemaMsgtype = @NameSpace + N'#' + @KeyName

DECLARE @guidDocSpecID uniqueidentifier
SELECT @guidDocSpecID = (SELECT TOP 1 bt_DocumentSpec.id 
       FROM bt_DocumentSpec INNER JOIN bts_assembly a ON a.nID = bt_DocumentSpec.assemblyid 
       WHERE bt_DocumentSpec.msgtype = @propSchemaMsgtype
       ORDER BY a.nVersionMajor DESC, a.nVersionMinor DESC, a.nVersionBuild DESC, a.nVersionRevision DESC -- get the newest
      )
IF (@guidDocSpecID IS NULL) 
BEGIN
 DECLARE @sensitivePropertyId int
 SELECT @sensitivePropertyId = (SELECT id FROM [bt_SensitiveProperties] WHERE msgtype =  @propSchemaMsgtype)
 IF( @sensitivePropertyId IS NULL )
  RETURN -1 -- property not found - fatal error
 RETURN -2 -- property cannot be promoted - it's security sensitive
END

DECLARE @aName nvarchar (4000)
SELECT @aName = nvcName FROM bts_assembly WHERE nID = @ModuleId

DECLARE @isTracked int
SELECT  @isTracked = is_tracked
 FROM bt_Properties p
  INNER JOIN bts_assembly a ON p.nAssemblyID = a.nID
 WHERE a.nID = ( SELECT TOP 1 nID
    FROM bts_assembly a1 
    WHERE a1.nvcName = @aName
    AND a1.nID <> @ModuleId
    ORDER BY a1.nVersionMajor DESC,a1.nVersionMinor DESC,a1.nVersionBuild  DESC,a1.nVersionRevision DESC -- get the newest
   ) AND
     p.namespace = @NameSpace AND
     p.name = @KeyName

IF ( @isTracked IS NULL )
 SELECT @isTracked = 0

INSERT INTO bt_Properties (
  nAssemblyID,
  propSchemaID,
  msgtype,
  namespace,
  name,
  xpath,
  is_tracked,
  itemid
 )
 VALUES (
  @ModuleId, 
  @guidDocSpecID,
  @MsgType,
  @NameSpace,
  @KeyName,
  @XPath,
  @isTracked,
  @ItemId
 )
RETURN 0 -- success
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_SaveSchemaProperty] TO [BTS_ADMIN_USERS]
    AS [dbo];

