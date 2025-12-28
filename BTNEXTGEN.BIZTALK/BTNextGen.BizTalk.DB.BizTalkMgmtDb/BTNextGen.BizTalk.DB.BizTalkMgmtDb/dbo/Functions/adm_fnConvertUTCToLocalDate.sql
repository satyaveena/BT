CREATE FUNCTION adm_fnConvertUTCToLocalDate (@dtParam datetime)
RETURNS datetime
AS
BEGIN

 declare @dtLocal datetime, @dtUTC datetime, @dtResult datetime

 SELECT @dtLocal = LocalDate FROM admv_LocalDate
 SELECT @dtUTC = UTCDate FROM admv_UTCDate

 set @dtResult = DATEADD(hour, DATEDIFF(hour, @dtUTC, @dtLocal), @dtParam)
 set @dtResult = DATEADD(minute, DATEDIFF(minute, @dtUTC, @dtLocal), @dtParam)

 return @dtResult
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_fnConvertUTCToLocalDate] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_fnConvertUTCToLocalDate] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_fnConvertUTCToLocalDate] TO [BTS_OPERATORS]
    AS [dbo];

