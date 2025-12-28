CREATE PROC [dbo].[xref_GetCommonID]
@idXRef  nvarchar(50),
@appInstance  nvarchar(50),
@appID  nvarchar(255),
@commonID nvarchar(50) OUTPUT,
@idAndAppExist  tinyint OUTPUT
AS
SET @idAndAppExist = 1
SELECT @commonID = ird.commonID
FROM xref_IDXRefData ird
    INNER JOIN xref_AppInstance ai ON ai.appInstanceID = ird.appInstanceID
 INNER JOIN xref_IDXRef ir ON ir.idXRefID = ird.idXRefID
WHERE ir.idXRef = @idXRef AND ai.appInstance = @appInstance AND ird.appID = @appID

IF @commonID is NULL
 IF (NOT EXISTS (SELECT [idXRefID] FROM xref_IDXRef WHERE idXRef = @idXRef) OR
  NOT EXISTS (SELECT [appInstanceID] FROM xref_AppInstance WHERE appInstance = @appInstance))
  SET @idAndAppExist = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xref_GetCommonID] TO [BTS_HOST_USERS]
    AS [dbo];

