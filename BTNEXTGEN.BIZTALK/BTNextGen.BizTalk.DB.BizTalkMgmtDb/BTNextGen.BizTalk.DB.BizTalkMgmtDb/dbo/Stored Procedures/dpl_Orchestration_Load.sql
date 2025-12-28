CREATE PROCEDURE [dbo].[dpl_Orchestration_Load] 
(
 @ArtifactId as int,
 @Id as int OUTPUT,
 @ServiceGuid as nvarchar(256) OUTPUT
)

AS
set nocount on
set xact_abort on

SELECT @Id = [bts_orchestration].[nID], @ServiceGuid = [bts_orchestration].[uidGUID]
FROM [bts_orchestration] 
 INNER JOIN bts_item ON ( [bts_orchestration].[nItemID] = [bts_item].[id] )
WHERE ([bts_orchestration].[nItemID] = @ArtifactId) AND ([bts_orchestration].[nvcFullName] = [bts_item].[FullName])
  
IF (@ServiceGuid IS NULL)
 RETURN -1
RETURN 1
 

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Load] TO [BTS_OPERATORS]
    AS [dbo];

