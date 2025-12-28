CREATE procedure [dbo].[bt_GetDocSpecInfoByMsgType]
@nvcMsgtype nvarchar(256),
@iPipelineAssemblyId int,
@iFlag int OUTPUT -- 0: No schema matched, 1: 1 schema matched, n > 1: multiple schema matched (ambiguity)
AS

--
-- Search by MsgType only
--
DECLARE @count int
SELECT  @count = COUNT(*)
FROM    bt_DocumentSpec d WITH (ROWLOCK),
  bt_XMLShare x WITH (ROWLOCK)
WHERE d.msgtype = @nvcMsgtype AND x.id = d.shareid AND
        x.active = 1

SET @iFlag = @count

IF (@count <= 1)
    -- At most one schema of the given msg type was found
 -- Thus, no duplicated schemas. 
 BEGIN

        SELECT  d.id, d.msgtype, d.docspec_name, d.clr_assemblyname
  FROM    bt_DocumentSpec d WITH (ROWLOCK),
          bt_XMLShare x WITH (ROWLOCK)
  WHERE d.msgtype = @nvcMsgtype AND x.id = d.shareid AND
                x.active = 1

  RETURN
 END
ELSE
 BEGIN
  --
  -- There are duplicated schemas
  -- 1. Pick the schema that's defined in the same assembly as the pipeline's
  -- 2. Otherwise, see if the schema is defined in an assembly with the
  --  same public key as the pipeline assembly's.
  --

        --
        -- Search by MsgType + AssemblyID
        --
        SELECT  @count = COUNT(*)
        FROM    bt_DocumentSpec d WITH (ROWLOCK),
          bt_XMLShare x WITH (ROWLOCK)
        WHERE d.msgtype = @nvcMsgtype AND x.id = d.shareid AND x.active = 1 AND d.assemblyid = @iPipelineAssemblyId 

  -- If there is 0 row returned, should not change the flag
  IF (@count <> 0)
   SET @iFlag = @count

        IF (@count = 1)
   BEGIN
    --
    -- Find the schema in the pipeline assembly
    -- No duplicated schemas can be defined in one assembly
    -- So ASSERT( @@ROWCOUNT == 1 ): This is THE one.       
    -- And, @iFlag is 1, which is the @count value
    --
    SELECT  d.id, d.msgtype, d.docspec_name, d.clr_assemblyname
    FROM    bt_DocumentSpec d WITH (ROWLOCK),
                  bt_XMLShare x WITH (ROWLOCK)
    WHERE d.msgtype = @nvcMsgtype AND x.id = d.shareid AND x.active = 1 AND d.assemblyid = @iPipelineAssemblyId 

    RETURN
   END
  ELSE
   BEGIN
       -- EAP Release Note: In EAP, same msgtype of different versions is not supported, thus TOP 1 and order by etc. are not implemented
       -- BTS 2004 SP1 Release Note: Same msgtype of different versions should work (though not side by side, just highest version), 
       -- based on examining bt_XMLShare.active flag
       
    -- 
    -- Look for schema in assemblies signed with pipeline assembly's public key
    -- If multiple are found, use the highest version
    -- The highest version coincides with the highest assemblyid,
    -- so we will ORDER BY assemblyid, and pick the TOP 1
    --

    -- find the public key of the pipeline assembly
    -- multiple may be returned. the caller will check the rowcount.
    DECLARE @nvcPublicKeyToken nvarchar(256)
    SELECT @nvcPublicKeyToken = a0.nvcPublicKeyToken
    FROM bts_assembly a0
    WHERE a0.nID = @iPipelineAssemblyId
                
    SELECT d.id, d.msgtype, d.docspec_name, d.clr_assemblyname
    FROM    bt_DocumentSpec d WITH (ROWLOCK)
            INNER JOIN bt_XMLShare x WITH (ROWLOCK) ON x.id = d.shareid
      INNER JOIN bts_assembly a WITH (ROWLOCK) ON d.assemblyid = a.nID
    WHERE d.msgtype = @nvcMsgtype AND
            x.active = 1 AND
      a.nvcPublicKeyToken = @nvcPublicKeyToken
    
    -- If there is 0 row returned, should not change the flag
    SET @count = @@ROWCOUNT
    IF (@count <> 0)
     SET @iFlag = @count
    
    RETURN
   END
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_GetDocSpecInfoByMsgType] TO [BTS_HOST_USERS]
    AS [dbo];

