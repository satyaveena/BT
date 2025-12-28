CREATE PROCEDURE [dbo].[sp_GetFileNameFromFilePath] @FilePath nvarchar(2000), @Name nvarchar(500) OUTPUT, @Location nvarchar(1500) OUTPUT
AS
 BEGIN
 set nocount on

 DECLARE @pos int
 
 IF @FilePath IS NULL OR len( @FilePath ) = 0
  RETURN -1

 SELECT @pos = len( @FilePath )
 
 WHILE @pos > 0
  BEGIN
  IF N'\' = substring( @FilePath, @pos, 1 )
   BREAK
 
  SET @pos = @pos - 1
  END

 IF @pos = 0
  RETURN -1
 
 SELECT @Name = substring( @FilePath, @pos+1, len( @FilePath ) - @pos + 1 )
 SELECT @Location = substring( @FilePath, 0, @pos )

 RETURN 0

 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetFileNameFromFilePath] TO [BTS_BACKUP_USERS]
    AS [dbo];

