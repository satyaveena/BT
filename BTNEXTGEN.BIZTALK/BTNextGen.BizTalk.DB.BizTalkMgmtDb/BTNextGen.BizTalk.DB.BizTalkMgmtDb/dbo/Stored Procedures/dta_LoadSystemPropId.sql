CREATE PROCEDURE [dbo].[dta_LoadSystemPropId]
@nvcPropName1 nvarchar(256),
@nvcPropName2 nvarchar(256),
@nvcPropName3 nvarchar(256),
@nvcPropName4 nvarchar(256),
@nvcPropName5 nvarchar(256),
@uidPropId1 uniqueidentifier output,
@uidPropId2 uniqueidentifier output,
@uidPropId3 uniqueidentifier output,
@uidPropId4 uniqueidentifier output,
@uidPropId5 uniqueidentifier output

AS
 set nocount on
 
 select @uidPropId1 = id from bt_DocumentSpec where msgtype = @nvcPropName1
 select @uidPropId2 = id from bt_DocumentSpec where msgtype = @nvcPropName2
 select @uidPropId3 = id from bt_DocumentSpec where msgtype = @nvcPropName3
 select @uidPropId4 = id from bt_DocumentSpec where msgtype = @nvcPropName4
 select @uidPropId5 = id from bt_DocumentSpec where msgtype = @nvcPropName5
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dta_LoadSystemPropId] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dta_LoadSystemPropId] TO [BTS_OPERATORS]
    AS [dbo];

