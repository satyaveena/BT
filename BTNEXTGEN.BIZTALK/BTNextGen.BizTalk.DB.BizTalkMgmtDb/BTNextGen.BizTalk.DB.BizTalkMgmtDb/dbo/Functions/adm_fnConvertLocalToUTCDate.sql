CREATE FUNCTION adm_fnConvertLocalToUTCDate (@dtParam datetime)
RETURNS datetime
AS
BEGIN

 declare @dtLocal datetime, @dtUTC datetime, @dtResult datetime

 SELECT @dtLocal = LocalDate FROM admv_LocalDate
 SELECT @dtUTC = UTCDate FROM admv_UTCDate

 set @dtResult = DATEADD(hour, DATEDIFF(hour, @dtLocal, @dtUTC), @dtParam)
 set @dtResult = DATEADD(minute, DATEDIFF(minute, @dtLocal, @dtUTC), @dtParam)

 return @dtResult
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_fnConvertLocalToUTCDate] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_fnConvertLocalToUTCDate] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_fnConvertLocalToUTCDate] TO [BTS_OPERATORS]
    AS [dbo];

