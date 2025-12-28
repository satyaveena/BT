CREATE PROCEDURE [dbo].[adm_Query_BizTalkDBVersion]
@BizTalkDBName nvarchar(64)
AS
 set nocount on
 set xact_abort on

 select top 1
  DatabaseMajor,
  DatabaseMinor,
  DatabaseBuildNumber,
  DatabaseRevision,
  ProductMajor, 
  ProductMinor, 
  ProductBuildNumber,
  ProductRevision,
  ProductLanguage,
  Description
 from
  BizTalkDBVersion
 where
  BizTalkDBName = @BizTalkDBName
 order by 
  ProductMajor desc, ProductMinor desc, ProductBuildNumber desc, ProductRevision desc

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Query_BizTalkDBVersion] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Query_BizTalkDBVersion] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Query_BizTalkDBVersion] TO [BTS_OPERATORS]
    AS [dbo];

