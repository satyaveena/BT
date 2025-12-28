CREATE FUNCTION [dbo].[adm_GetGroupId] ()
RETURNS int
AS
BEGIN
 declare @GroupId as int
 select @GroupId = Id from adm_Group

 return @GroupId
END