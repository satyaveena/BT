create procedure [dbo].[adm_Host_Register_TDDS_Services]
@Name nvarchar(256),
@Register int
as
 declare @SvrName as nvarchar(256)
 declare @SvcId as uniqueidentifier

 declare @ErrCode as int
 select @ErrCode = 0
 
 DECLARE HostInstList CURSOR FOR
 select Svr.Name, HostInst.UniqueId 
 from 
  adm_HostInstance HostInst,
  adm_Server2HostMapping Mapping,
  adm_Server Svr,
  adm_Host Host
 where 
  HostInst.Svr2HostMappingId = Mapping.Id and
  Mapping.ServerId = Svr.Id and
  Mapping.HostId = Host.Id and
  Host.Name = @Name

 OPEN HostInstList
 FETCH NEXT FROM HostInstList into @SvrName, @SvcId
 WHILE ((@@fetch_status <> -1))
 BEGIN
  -- register/unregister server for given host instance
  declare @GroupName nvarchar(256)
  set @GroupName = dbo.adm_GetGroupName()

  if ( @Register <> 0 )
   begin
    exec @ErrCode = TDDS_RegisterService @SvcId, @SvrName
   end
  else
   begin
    exec @ErrCode = TDDS_UnregisterService @SvcId
   end 
   
  --return if error
  if ( @ErrCode <> 0 )
   break;
  
  FETCH NEXT FROM HostInstList into @SvrName, @SvcId
 END
 
 CLOSE HostInstList
 DEALLOCATE HostInstList

 if ( @ErrCode <> 0 )
  return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Host_Register_TDDS_Services] TO [BTS_ADMIN_USERS]
    AS [dbo];

