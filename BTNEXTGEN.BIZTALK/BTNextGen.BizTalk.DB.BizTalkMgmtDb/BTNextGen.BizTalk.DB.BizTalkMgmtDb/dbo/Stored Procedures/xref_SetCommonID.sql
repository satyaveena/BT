CREATE PROC [dbo].[xref_SetCommonID]
@idXRef  nvarchar(50),
@appInstance  nvarchar(50),
@appID  nvarchar(255),
@commonID nvarchar(50) OUTPUT,
@idAndAppExist tinyint OUTPUT
AS
DECLARE @idXRefID int
DECLARE @appInstanceID int

SELECT @idXRefID = idXRefID FROM xref_IDXRef WHERE idXRef = @idXRef

SELECT @appInstanceID = appInstanceID FROM xref_AppInstance WHERE appInstance = @appInstance

IF @idXRefID IS NULL OR @appInstanceID IS NULL
BEGIN
 SET @idAndAppExist = 0
 RETURN
END

SET @idAndAppExist = 1
DECLARE @tempCommonID nvarchar(50)
SELECT @tempCommonID = commonID
 FROM xref_IDXRefData
 WHERE idXRefID = @idXRefID AND appInstanceID = @appInstanceID AND appID = @appID
IF @tempCommonID is NULL
INSERT xref_IDXRefData (idXRefID, appInstanceID, appID, commonID)
VALUES (@idXRefID, @appInstanceID, @appID, @commonID)
ELSE
SET @commonID = @tempCommonID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xref_SetCommonID] TO [BTS_HOST_USERS]
    AS [dbo];

