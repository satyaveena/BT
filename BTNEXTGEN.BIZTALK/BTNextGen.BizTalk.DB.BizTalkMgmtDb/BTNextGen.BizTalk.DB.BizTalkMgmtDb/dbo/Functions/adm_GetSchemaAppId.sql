CREATE FUNCTION [dbo].[adm_GetSchemaAppId] (@schemaId uniqueidentifier) RETURNS int
AS
BEGIN
	declare @nAppId as int
	select @nAppId = bts_assembly.nApplicationID 
		from bt_MapSpec, bts_assembly 
		where bt_MapSpec.id=@schemaId 
		and bt_MapSpec.assemblyid=bts_assembly.nID
	return @nAppId
END
