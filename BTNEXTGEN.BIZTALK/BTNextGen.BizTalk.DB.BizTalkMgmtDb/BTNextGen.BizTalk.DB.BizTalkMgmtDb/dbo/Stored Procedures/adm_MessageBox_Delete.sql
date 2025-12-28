CREATE PROCEDURE [dbo].[adm_MessageBox_Delete]
@DBServerName nvarchar(80),
@DBName nvarchar(128)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0
 
 -- Constraint checking before removing msgbox record
 declare @DisableNewMsgPublication as int
 declare @DateModified as datetime

 select @DisableNewMsgPublication = DisableNewMsgPublication,
   @DateModified = DateModified
 from adm_MessageBox
 where DBServerName = @DBServerName AND
   DBName = @DBName

 declare @ConfigurationCacheRefreshInterval as int

 select 
  @ConfigurationCacheRefreshInterval = ConfigurationCacheRefreshInterval
 from
  adm_Group Grp

 -- The constraints are as follows:
 -- 1) The messagebox must be marked as Disable New Message Publication
 -- 2) At least two runtime cache refresh interval has passed since the messagebox
 --    was marked as Disable New Message Publication.  This limitation minimizes the
 --    chance that a host instance could send new work into a messagebox after the
 --    user has initiated deletion.  
 if ( @DisableNewMsgPublication <> -1 )
  set @ErrCode = 0xC0C025BC -- CIS_E_ADMIN_CORE_MSGBOX_DELETE_PUBLICATION_NOT_DISABLED
 else if ( DATEDIFF(ss, @DateModified, GETUTCDATE()) < (@ConfigurationCacheRefreshInterval * 2))
  set @ErrCode = 0xC0C025BB -- CIS_E_ADMIN_CORE_MSGBOX_DELETE_FAIL_WORK_REMAIN

 if ( @ErrCode <> 0 ) goto exit_proc
 
 -- Invoke internal method to remove msgbox record and related records
 exec @ErrCode = adm_MessageBox_Internal_Delete @DBServerName, @DBName
 if ( @ErrCode <> 0 ) goto exit_proc

exit_proc:
 return @ErrCode
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

