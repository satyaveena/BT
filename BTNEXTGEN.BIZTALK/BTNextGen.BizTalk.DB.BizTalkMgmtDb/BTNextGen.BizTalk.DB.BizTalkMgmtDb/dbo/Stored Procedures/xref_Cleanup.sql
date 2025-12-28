CREATE PROC [dbo].[xref_Cleanup]
AS
TRUNCATE TABLE [dbo].[xref_AppInstance]
TRUNCATE TABLE [dbo].[xref_AppType]
TRUNCATE TABLE [dbo].[xref_IDXRef]
INSERT INTO [dbo].[xref_IDXRef] (idXRef)
VALUES (N'')
TRUNCATE TABLE [dbo].[xref_IDXRefData]
TRUNCATE TABLE [dbo].[xref_MessageArgument]
TRUNCATE TABLE [dbo].[xref_MessageDef]
TRUNCATE TABLE [dbo].[xref_MessageText]
TRUNCATE TABLE [dbo].[xref_ValueXRef]
INSERT INTO [xref_ValueXRef] (valueXRefName)
VALUES (N'')
TRUNCATE TABLE [dbo].[xref_ValueXRefData]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xref_Cleanup] TO [BTS_ADMIN_USERS]
    AS [dbo];

