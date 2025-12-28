CREATE PROCEDURE [dbo].[dpl_IsSystemAssembly]
(
 @Name nvarchar(256)
)
AS

DECLARE @SystemAssembly int

IF ( @Name LIKE N'Microsoft.BizTalk.DefaultPipelines,%PublicKeyToken=31bf3856ad364e35%' OR
  @Name LIKE N'Microsoft.BizTalk.GlobalPropertySchemas,%PublicKeyToken=31bf3856ad364e35%' OR
  @Name LIKE N'Microsoft.BizTalk.Hws.HwsPromotedProperties,%PublicKeyToken=31bf3856ad364e35%' OR
  @Name LIKE N'Microsoft.BizTalk.Hws.HwsSchemas,%PublicKeyToken=31bf3856ad364e35%' OR
  @Name LIKE N'Microsoft.BizTalk.KwTpm.RoleLinkTypes,%PublicKeyToken=31bf3856ad364e35%' OR
  @Name LIKE N'Microsoft.BizTalk.KwTpm.StsDefaultPipelines,%PublicKeyToken=31bf3856ad364e35%' OR
  @Name LIKE N'Microsoft.BizTalk.Adapter.MSMQ.MsmqAdapterProperties,%PublicKeyToken=31bf3856ad364e35%' OR
  @Name LIKE N'MQSeries,%PublicKeyToken=31bf3856ad364e35%' 
   )
    SELECT @SystemAssembly = 1
ELSE
    SELECT @SystemAssembly = 0

RETURN @SystemAssembly
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_IsSystemAssembly] TO [BTS_ADMIN_USERS]
    AS [dbo];

