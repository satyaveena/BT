CREATE PROC [dbo].[xref_GetAppValue]
@valueXRef   nvarchar(50),
@appType   nvarchar(50),
@commonValue  nvarchar(50),
@appValue  nvarchar(50) OUTPUT,
@valueAndAppExist tinyint OUTPUT
AS
DECLARE @valueXRefID int
DECLARE @appTypeID int

SELECT @valueXRefID = valueXRefID FROM xref_ValueXRef WHERE valueXRefName = @valueXRef

SELECT @appTypeID = appTypeID FROM xref_AppType WHERE appType = @appType

IF @valueXRefID IS NULL OR @appTypeID IS NULL
BEGIN
 SET @valueAndAppExist = 0
 RETURN
END
SET @valueAndAppExist = 1
SELECT @appValue = appValue
FROM xref_ValueXRefData
WHERE valueXRefID = @valueXRefID AND appTypeID = @appTypeID AND commonValue = @commonValue
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xref_GetAppValue] TO [BTS_HOST_USERS]
    AS [dbo];

