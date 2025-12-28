CREATE PROCEDURE [dbo].[adm_AcquireAppLock]
@ResourceName nvarchar(255),
@WaitForLock int
AS
 set nocount on

 declare @LockResult as int, @TimeOut as int
 select @LockResult = 0, @TimeOut = @@LOCK_TIMEOUT

 if ( @WaitForLock = 0 )
 begin
  -- Set @TimeOut value so that lock request that cannot be granted immediately
  -- should return right away
  set @TimeOut = 0
 end

 -- Acquire applock using input ResourceName
 exec @LockResult = sp_getapplock @ResourceName, 'Exclusive', 'Transaction', @TimeOut

 -- return the lock result back
 select @LockResult

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_AcquireAppLock] TO [BTS_ADMIN_USERS]
    AS [dbo];

