CREATE PROCEDURE [dbo].[adm_ChangeHostTrackingPrivilege]
@ApplicationAccount nvarchar(128),
@ProtectType nvarchar(10) -- GRANT/DENY/REVOKE
AS
 declare @ErrCode as int
 set @ErrCode = 0

 exec @ErrCode = adm_ChangeRolePrivForUser 'BAM_EVENT_WRITER', @ApplicationAccount, @ProtectType
 if(@ErrCode != 0)
  return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ChangeHostTrackingPrivilege] TO [BTS_ADMIN_USERS]
    AS [dbo];

