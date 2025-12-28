CREATE PROC [dbo].[xref_GetAppID]
@idXRef  nvarchar(50),
@appInstance  nvarchar(50),
@commonID nvarchar(50),
@appID  nvarchar(255) OUTPUT,
@idAndAppExist  tinyint OUTPUT
AS
SET @idAndAppExist = 1

SELECT @appID = ird.appID  
FROM xref_IDXRefData ird 
    INNER JOIN xref_AppInstance ai ON ai.appInstanceID = ird.appInstanceID
 INNER JOIN xref_IDXRef ir ON ir.idXRefID = ird.idXRefID
WHERE ir.idXRef = @idXRef AND ai.appInstance = @appInstance AND ird.commonID = @commonID

IF @appID is NULL
 IF (NOT EXISTS (SELECT [idXRefID] FROM xref_IDXRef WHERE idXRef = @idXRef) OR
  NOT EXISTS (SELECT [appInstanceID] FROM xref_AppInstance WHERE appInstance = @appInstance))
  SET @idAndAppExist = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xref_GetAppID] TO [BTS_HOST_USERS]
    AS [dbo];

