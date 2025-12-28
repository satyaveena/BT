CREATE PROCEDURE [dbo].[dpl_Orchestration_Save] 
(
 @ModuleId as int,
 @ArtifactId as int,
 @ServiceGuid as nvarchar(256),
 @Namespace as nvarchar(256),
 @Name as nvarchar(256),
 @FullName as nvarchar(256),
 @ServiceTypeGuid as nvarchar(256),
 @OrchestrationInfo as int
)

AS
set nocount on
set xact_abort on

INSERT INTO [bts_orchestration]
 (
  uidGUID,
  nAssemblyID, 
  nItemID,
  nvcNamespace,
  nvcName,
  nOrchestrationInfo,
  nOrchestrationStatus,
  dtModified
 )
 VALUES
 (
  convert(uniqueidentifier,@ServiceGuid),
  @ModuleId,
  @ArtifactId,
  @Namespace,
  @Name,
  @OrchestrationInfo,
  1, -- 0=Invalid, 1=Unenlisted, 2=Enlisted, 3=Started; new service is always Unenlisted.
  GETUTCDATE()
 )

RETURN @@IDENTITY
 
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

