CREATE PROCEDURE [dbo].[MBOM_GetMessageBoxes]
AS
	SET NOCOUNT ON
	select
		adm_MessageBox.DBServerName, 
		adm_MessageBox.DBName
	from
		adm_MessageBox
	RETURN 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MBOM_GetMessageBoxes] TO [BTS_ADMIN_USERS]
    AS [dbo];

