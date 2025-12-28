CREATE PROC [dbo].[xref_GetCommonValue]
@valueXRef   nvarchar(50),
@appType   nvarchar(50),
@appValue  nvarchar(50),
@commonValue  nvarchar(50) OUTPUT,
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

SELECT @commonValue = commonValue
FROM xref_ValueXRefData
WHERE valueXRefID = @valueXRefID AND appTypeID = @appTypeID AND appValue = @appValue
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xref_GetCommonValue] TO [BTS_HOST_USERS]
    AS [dbo];

