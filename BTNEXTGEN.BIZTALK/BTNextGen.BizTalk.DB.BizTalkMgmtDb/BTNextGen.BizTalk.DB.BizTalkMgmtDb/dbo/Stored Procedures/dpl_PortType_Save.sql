CREATE PROCEDURE [dbo].[dpl_PortType_Save]
(
 @ModuleID as int,
 @ArtifactID as int,
 @Name as nvarchar(256),
 @Namespace as nvarchar(256)
)

AS
set nocount on
set xact_abort on

INSERT INTO bts_porttype
 ( 
  nAssemblyID,
  nvcName,
  nvcNamespace
 )
VALUES
 (
  @ModuleID,
  @Name,
  @Namespace
 )

RETURN @@IDENTITY

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_PortType_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

