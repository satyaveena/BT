CREATE PROCEDURE [dbo].[bt_GetPipelineIdFromName]
@nvcAssemblyName nvarchar(256),
@iId int OUTPUT
AS
 set nocount on
 
 SELECT @iId = nID
 FROM bts_assembly
 WHERE nvcFullName = @nvcAssemblyName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bt_GetPipelineIdFromName] TO [BTS_HOST_USERS]
    AS [dbo];

