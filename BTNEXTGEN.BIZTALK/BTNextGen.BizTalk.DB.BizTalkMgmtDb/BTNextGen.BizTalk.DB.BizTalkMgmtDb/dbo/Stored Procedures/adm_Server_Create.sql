CREATE PROCEDURE [dbo].[adm_Server_Create]
@Name nvarchar(63)
AS
 set nocount on
 set xact_abort on

 begin transaction

  declare @BIZTALK_UNKNOWN_DBID int
  set @BIZTALK_UNKNOWN_DBID = 0

  declare @ServerId as int
  set @ServerId = @BIZTALK_UNKNOWN_DBID

  -- Resolve foreign key: ServerId 
  select @ServerId = Id
  from adm_Server
  where Name = @Name

  -- Create new adm_Server record
  insert into adm_Server
  (
   Name
  )
  values
  (
   @Name
  )

  set @ServerId = @@IDENTITY

  -- Create the necessary adm_Server2HostMapping records
  insert into adm_Server2HostMapping
   (ServerId, HostId, IsMapped)
  select
   @ServerId,
   adm_Host.Id,
   0  -- by default new Server is never Mapped
  from
   adm_Host

 commit transaction

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server_Create] TO [BTS_ADMIN_USERS]
    AS [dbo];

