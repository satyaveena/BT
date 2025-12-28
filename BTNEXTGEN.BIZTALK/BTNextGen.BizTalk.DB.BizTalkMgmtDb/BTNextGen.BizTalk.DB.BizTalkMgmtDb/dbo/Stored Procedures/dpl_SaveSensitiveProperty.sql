CREATE PROCEDURE [dbo].[dpl_SaveSensitiveProperty]
(
 @ModuleId int,
 @MsgType nvarchar(4000)
)

AS


INSERT INTO bt_SensitiveProperties (
  assemblyid,
  msgtype
 )
 VALUES (
  @ModuleId, 
  @MsgType
 )
RETURN 0 -- success
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_SaveSensitiveProperty] TO [BTS_ADMIN_USERS]
    AS [dbo];

