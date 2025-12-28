CREATE PROCEDURE [dbo].[dpl_DeleteAssembly]
(
 @Guid nvarchar(256),
 @Name nvarchar(256),
 @VersionMajor int,
 @VersionMinor int,
 @VersionBuild int, 
 @VersionRevision int, 
 @PublicKeyToken nvarchar(256),
 @Culture nvarchar(256),
 @Type int,
 @NoSchemasVerify int
)

AS
set nocount on

DECLARE @ModuleId int
DECLARE @SystemAssembly int
SELECT @ModuleId = nID, @SystemAssembly = nSystemAssembly
FROM bts_assembly
WHERE
 (@Name = nvcName) and
 (@VersionMajor = nVersionMajor) and
 (@VersionMinor = nVersionMinor) and
 (@VersionBuild = nVersionBuild) and
 (@VersionRevision = nVersionRevision) and
 (@PublicKeyToken = nvcPublicKeyToken) and
 (@Type = nType) 
IF (@@ROWCOUNT > 0 and @SystemAssembly = 0)
 BEGIN
  DELETE FROM bt_Properties WHERE @ModuleId = nAssemblyID
  DELETE FROM bt_MapSpec WHERE @ModuleId = assemblyid

  -- Delete schemas; ref count to account for CLR type schemas

  DELETE FROM bts_libreference WHERE idapp = @ModuleId OR idlib = @ModuleId
  DELETE FROM bts_itemreference WHERE nReferringAssemblyID = @ModuleId

  DELETE bt_DocumentSpec 
   WHERE (assemblyid = @ModuleId)
  DELETE bt_DocumentSpec 
   WHERE (assemblyid NOT IN (SELECT idlib FROM bts_libreference))  AND itemid IN (SELECT id FROM bts_item WHERE SchemaType = 2)
   -- cleanup unreferenced XMLShare rows
  DELETE FROM bt_XMLShare WHERE NOT EXISTS (SELECT shareid from bt_DocumentSpec WHERE bt_XMLShare.id = shareid) AND
           NOT EXISTS (SELECT shareid from bt_MapSpec WHERE bt_XMLShare.id = shareid)
           
  DELETE FROM bts_item where SchemaType = 2 AND id NOT IN (SELECT itemid FROM bt_DocumentSpec) 
  
  DELETE FROM bts_assembly WHERE nID NOT IN (SELECT AssemblyId FROM bts_item) AND nSystemAssembly <> 0 -- cleanup phantom assembly rows if unreferenced anymore
           
  -- Deactivate previous active schema version
  UPDATE bt_XMLShare
   SET active = 0
   WHERE id IN ( SELECT DISTINCT xs.id 
       FROM bt_XMLShare xs INNER JOIN bt_DocumentSpec ds ON xs.id = ds.shareid
            INNER JOIN bts_assembly md ON ds.assemblyid = md.nID
       WHERE md.nvcName = @Name AND
         xs.active = 1
      )

    
  ---------------- delete pipelines -------------------
  DELETE FROM bts_component WHERE Id IN 
   (SELECT CompID FROM bts_stage_config sc
         INNER JOIN bts_pipeline_config pc ON sc.StageID = pc.StageID
         INNER JOIN bts_pipeline p ON pc.PipelineID = p.Id
           WHERE p.nAssemblyID = @ModuleId )

  DELETE FROM bts_pipeline_stage WHERE Id IN 
   (SELECT sc.StageID FROM bts_stage_config sc
         INNER JOIN bts_pipeline_config pc ON sc.StageID = pc.StageID
         INNER JOIN bts_pipeline p ON pc.PipelineID = p.Id
           WHERE p.nAssemblyID = @ModuleId )

  DELETE FROM bts_stage_config WHERE StageID IN 
   (SELECT StageID FROM bts_pipeline_config pc 
         INNER JOIN bts_pipeline p ON pc.PipelineID = p.Id
           WHERE p.nAssemblyID = @ModuleId )

  DELETE FROM bts_pipeline_config WHERE PipelineID IN 
   (SELECT Id FROM bts_pipeline p
           WHERE p.nAssemblyID = @ModuleId )

  DELETE FROM bts_pipeline WHERE nAssemblyID = @ModuleId 
  -----------------------------------------------------
  ---------------- delete services --------------------
  /* remove bts_orchestration_port rows
   handled by CASCADE DELETE constraint
  */
  
  /* remove bts_rolelink rows
  handled by CASCADE DELETE constraint
  */
  
  /* verify that there are no active bindings pointing to any service in this assembly */
  DECLARE @bindcount int
  SELECT @bindcount = COUNT(o.nID) 
  FROM bts_orchestration_port_binding opb
   INNER JOIN bts_orchestration_port op ON op.nID = opb.nOrcPortID
   INNER JOIN bts_orchestration o ON op.nOrchestrationID = o.nID
   INNER JOIN bts_assembly a ON a.nID = o.nAssemblyID
  WHERE a.nID = @ModuleId AND
   ( opb.nReceivePortID IS NOT NULL OR
    opb.nSendPortID IS NOT NULL OR
    opb.nSpgID IS NOT NULL 
   )
  IF ( @bindcount > 0 )
   RETURN -3
  

--    DELETE FROM StaticTrackingInfo
--    WHERE uidServiceID IN (
--      SELECT map.uidOrchestrationType
--     FROM bts_assembly_orchestration_mapping map join bts_orchestration on (nOrchestrationID = nID)
-- 
--     WHERE (map.nAssemblyID = @ModuleId)
--    ) 
      
  -- remove service
  DELETE FROM bts_orchestration 
  WHERE nAssemblyID = @ModuleId
  -------------------------------------------------------------------------------------

  -------------------- Delete servicelinktypes and related subitems -------------------
  /* bts_role rows deletion handled by CASCADE DELETE constraint
  */
  /* bts_role_porttype deletion handled by CASCADE DELETE constraint
  */
  DELETE FROM bts_rolelink_type 
  WHERE nAssemblyID = @ModuleId
  -----------------------------------------------------------------------------

  --------------------------- Delete porttypes  ----------------------
  /* bts_porttype_operation rows deletion handled by CASCADE DELETE constraint
  */
  DELETE FROM bts_porttype
  WHERE nAssemblyID = @ModuleId
  -----------------------------------------------------------------------------
     
  --------------------------- Delete messagetypes ----------------------
  /* bts_messagetype_part rows deletion handled by CASCADE DELETE constraint
  */
  DELETE FROM bts_messagetype 
  WHERE nAssemblyID = @ModuleId
  -----------------------------------------------------------------------------


  DELETE FROM bts_item WHERE @ModuleId = AssemblyId
        
  /* finally, delete module row */
  
  DELETE FROM bts_assembly
   WHERE @ModuleId = nID 


  -- Assembly is gone - now activate all schemas coming from assembly with the highest version number
  UPDATE bt_XMLShare 
   SET active = 1
   FROM bt_XMLShare x INNER JOIN bt_DocumentSpec ds ON x.id = ds.shareid
   WHERE ds.id IN 
    ( SELECT TOP 1 id FROM bt_DocumentSpec d
        INNER JOIN bts_assembly md ON ds.assemblyid = md.nID
     WHERE md.nvcName = @Name AND
        d.msgtype = ds.msgtype
     ORDER BY md.nVersionMajor DESC, md.nVersionMinor DESC, md.nVersionBuild DESC, md.nVersionRevision DESC
    )

     
  IF ( @NoSchemasVerify = 0 )
  BEGIN
   -- Now verify and update references from bt_Properties to bt_DocumentSpec - no document property might be orphaned by 
   -- property schema downgrade
   DECLARE @orphancount int
   SELECT @orphancount = dbo.dpl_fn_CountOrphanedProperties()
   IF ( @orphancount > 0 )
    RETURN -2
  END
  RETURN 1
 END
ELSE
 BEGIN
  IF( @SystemAssembly = 0)
   RETURN -4
  ELSE
   RETURN -5
 END

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_DeleteAssembly] TO [BTS_ADMIN_USERS]
    AS [dbo];

