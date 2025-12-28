CREATE PROCEDURE [dbo].[adm_MessageBox_Load]
@DBServerName nvarchar(80),
@DBName nvarchar(128)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 select
  adm_MessageBox.Id,
  adm_MessageBox.DBServerName, 
  adm_MessageBox.DBName, 
  adm_MessageBox.DateModified, 
  adm_MessageBox.DisableNewMsgPublication,
  adm_MessageBox.ConfigurationState,
  adm_MessageBox.IsMasterMsgBox,
  adm_MessageBox.nvcDescription
 from
  adm_MessageBox
 where
  adm_MessageBox.DBServerName = @DBServerName AND
  adm_MessageBox.DBName = @DBName

 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_Load] TO [BTS_OPERATORS]
    AS [dbo];

