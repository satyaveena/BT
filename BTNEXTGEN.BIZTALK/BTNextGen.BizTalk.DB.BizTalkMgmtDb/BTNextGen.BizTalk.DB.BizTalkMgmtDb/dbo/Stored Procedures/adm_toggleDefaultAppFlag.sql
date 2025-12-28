CREATE PROCEDURE [dbo].[adm_toggleDefaultAppFlag]
AS
UPDATE bts_application SET isDefault = isDefault - 1