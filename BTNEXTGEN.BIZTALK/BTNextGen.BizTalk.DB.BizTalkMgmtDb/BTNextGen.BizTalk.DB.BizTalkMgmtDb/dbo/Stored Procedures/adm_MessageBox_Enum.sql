CREATE PROCEDURE [dbo].[adm_MessageBox_Enum]
AS
 set nocount on
 set xact_abort on

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

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_Enum] TO [BTS_OPERATORS]
    AS [dbo];

