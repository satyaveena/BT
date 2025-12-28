CREATE PROCEDURE [dbo].[admsvr_GetDBIDByName]
@nvcDBServerName nvarchar(80),
@nvcDBName nvarchar(128),
@uidDBID uniqueidentifier output
AS
   set @uidDBID = null
	select @uidDBID = UniqueId from adm_MessageBox 
	where DBServerName = @nvcDBServerName AND
		  DBName = @nvcDBName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetDBIDByName] TO [BTS_HOST_USERS]
    AS [dbo];

