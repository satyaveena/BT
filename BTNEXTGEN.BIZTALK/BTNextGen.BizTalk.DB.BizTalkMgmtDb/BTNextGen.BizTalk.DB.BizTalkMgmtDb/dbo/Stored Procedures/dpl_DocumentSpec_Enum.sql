CREATE PROCEDURE [dbo].[dpl_DocumentSpec_Enum]
(
 @ModuleId as int
)

AS
set nocount on

select
 [bt_DocumentSpec].[id] as [Guid],
 [msgtype] as [MsgType],
 [docspec_name] as [Name],
 [body_xpath] as [BodyXpath],
 [target_namespace] as [TargetNamespace],
 [content] as [Content]
from [bt_DocumentSpec] join [bt_XMLShare] on [shareid] = [bt_XMLShare].[id]
where ([assemblyid] = @ModuleId)
order by [docspec_name]
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_DocumentSpec_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_DocumentSpec_Enum] TO [BTS_OPERATORS]
    AS [dbo];

