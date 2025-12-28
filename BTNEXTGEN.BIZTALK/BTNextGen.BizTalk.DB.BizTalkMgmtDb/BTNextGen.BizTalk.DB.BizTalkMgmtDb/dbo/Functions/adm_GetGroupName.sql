CREATE FUNCTION [dbo].[adm_GetGroupName] ()
RETURNS nvarchar(256)
AS
BEGIN
 declare @GroupName as nvarchar(256)
 select @GroupName = Name from adm_Group

 return @GroupName
END