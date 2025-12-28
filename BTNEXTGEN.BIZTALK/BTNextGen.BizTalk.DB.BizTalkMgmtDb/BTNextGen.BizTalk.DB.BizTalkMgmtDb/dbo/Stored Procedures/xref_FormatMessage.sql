CREATE PROC [dbo].[xref_FormatMessage]
@code   nvarchar(50),
@lang   nvarchar(50)
AS
DECLARE @msgID int
SELECT @msgID = msgID
FROM xref_MessageDef
WHERE msgCode = @code
IF @msgID is NULL
BEGIN
 RETURN
END
-- get msgText
SELECT msgText
FROM xref_MessageText
WHERE msgID = @msgID AND lang = @lang
-- get msgArgs
SELECT ma.argName, id.idXRef, v.valueXRefName
FROM xref_MessageArgument ma
    INNER JOIN xref_IDXRef id ON ma.argIDXRefID = id.idXRefID
 INNER JOIN xref_ValueXRef v ON ma.argValueXRefID = v.valueXRefID
WHERE ma.msgID = @msgID
ORDER BY ma.argSequenceNum
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xref_FormatMessage] TO [BTS_HOST_USERS]
    AS [dbo];

