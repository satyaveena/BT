CREATE FUNCTION [dbo].[edi_CompareStrings]
(@string1 nvarchar(256), @string2 nvarchar(256))
RETURNS int
AS
BEGIN
	Set @string1 = RTRIM(LTRIM(@string1))
	Set @string2 = RTRIM(LTRIM(@string2))
	if (@string1 is null and @string2 is null) return 0
	if (@string1 is null and @string2 = '') return 0
	if (@string1 = '' and @string2 is null) return 0
	if (@string1 = @string2) return 0
	return -1

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CompareStrings] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CompareStrings] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CompareStrings] TO [BTS_OPERATORS]
    AS [dbo];

