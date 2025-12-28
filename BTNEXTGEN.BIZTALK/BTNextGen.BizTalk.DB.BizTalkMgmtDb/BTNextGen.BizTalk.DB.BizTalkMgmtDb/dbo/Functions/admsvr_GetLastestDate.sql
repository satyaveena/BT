CREATE FUNCTION [dbo].[admsvr_GetLastestDate] (@date1 datetime, @date2 datetime)
RETURNS datetime
AS
BEGIN
	declare @largestDate datetime
	IF ( @date1 > @date2 )
		set @largestDate = @date1
	ELSE
		set @largestDate = @date2
	RETURN @largestDate
END
