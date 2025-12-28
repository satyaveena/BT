CREATE PROCEDURE [dbo].[dpl_Orchestration_Invocation_Save] 
(
 @InvokingServiceArtifactId as int,
 @InvokedServiceName as nvarchar(256),
 @InvokedServiceAssembly as nvarchar(256),
 @InvokeType as int,
 @VersionMajor as int,
 @VersionMinor as int ,
 @VersionBuild as int ,
 @VersionRevision as int
)

AS
set nocount on
set xact_abort on

DECLARE @Id int
SELECT @Id = nID
FROM bts_assembly
WHERE
 (@InvokedServiceAssembly = nvcName) and
 (@VersionMajor = nVersionMajor) and
 (@VersionMinor = nVersionMinor) and
 (@VersionBuild = nVersionBuild) and
 (@VersionRevision = nVersionRevision) and
 (2 = nType) /* Still needed - ignore hidden assemblies */

IF (@@ROWCOUNT = 0)
 RETURN -1
 
DECLARE @InvokedId int 
SELECT @InvokedId = nID 
 FROM bts_orchestration
 WHERE nAssemblyID = @Id AND 
    nvcName = @InvokedServiceName
            
IF (@@ROWCOUNT = 0)
 RETURN -2
 
DECLARE @InvokingServiceId int

SELECT @InvokingServiceId = nID FROM bts_orchestration WHERE nItemID = @InvokingServiceArtifactId
 
INSERT INTO bts_orchestration_invocation(
 nOrchestrationID ,
 nInvokedOrchestrationID ,
 nInvokeType /* 0 - invalid, 1 - call, 2 - exec */
 ) VALUES (
  @InvokingServiceId,
  @InvokedId,
  @InvokeType
 )


RETURN 0 /* OK */

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Invocation_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

