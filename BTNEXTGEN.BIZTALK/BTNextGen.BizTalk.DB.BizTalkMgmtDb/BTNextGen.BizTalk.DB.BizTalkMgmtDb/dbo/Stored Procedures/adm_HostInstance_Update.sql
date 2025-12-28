CREATE PROCEDURE [dbo].[adm_HostInstance_Update]
@Name nvarchar(256),
@DisableHostInstance int,
@LoginName nvarchar(128),
@InstallationContext nvarchar(256),
@Description nvarchar(256),
@CLRMaxWorkerThreads int,
@CLRMinWorkerThreads int,
@CLRMaxIOThreads int,
@CLRMinIOThreads int,
@PhysicalMemoryOptimalUsage int,
@PhysicalMemoryMaximalUsage int,
@VirtualMemoryOptimalUsage int,
@VirtualMemoryMaximalUsage int
AS
 set nocount on
 set xact_abort on
 declare @ErrCode as int
 set @ErrCode = 0

 if (@VirtualMemoryOptimalUsage <> 0 and @VirtualMemoryMaximalUsage <> 100 and @VirtualMemoryMaximalUsage <= @VirtualMemoryOptimalUsage)
 begin
     Set @ErrCode = 0xC0C02602  -- CIS_E_ADMIN_CORE_VIRTUAL_MEMORY_MAXIMAL_LESS_OR_EQUAL_TO_VIRTUAL_MEMORY_OPTIMAL
     return @ErrCode
 end

 if (@PhysicalMemoryOptimalUsage <> 0 and @PhysicalMemoryMaximalUsage <> 100 and @PhysicalMemoryMaximalUsage <= @PhysicalMemoryOptimalUsage)
 begin
     Set @ErrCode = 0xC0C02603  -- CIS_E_ADMIN_CORE_PHYSICAL_MEMORY_MAXIMAL_LESS_OR_EQUAL_TO_PHYSICAL_MEMORY_OPTIMAL
     return @ErrCode
 end
 
 if (@CLRMaxWorkerThreads < @CLRMinWorkerThreads)
 begin
     Set @ErrCode = 0xC0C02606 -- CIS_E_ADMIN_CORE_CLR_MAX_WORKER_THREADS_LESS_THAN_CLR_MIN_WORKER_THREADS
     return @ErrCode
 end
 
 if (@CLRMaxIOThreads < @CLRMinIOThreads)
 begin
     Set @ErrCode = 0xC0C02607 -- CIS_E_ADMIN_CORE_CLR_MAX_IO_THREADS_LESS_THAN_CLR_MIN_IO_THREADS
     return @ErrCode
 end
 
 begin transaction
  -- sometimes this store proc is called without logon (e.g. during uninstall)
  if(@InstallationContext='' OR @InstallationContext=NULL)
  begin
   select @InstallationContext=InstallationContext
   from adm_HostInstance
   where adm_HostInstance.Name = @Name
   set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
   if ( @ErrCode <> 0 ) goto exit_proc
  end
   
  update adm_HostInstance
  set
   DisableHostInstance = @DisableHostInstance,
   DateModified = GETUTCDATE(),
   LoginName = @LoginName,
   InstallationContext = @InstallationContext,
   nvcDescription = @Description
  where
   Name = @Name
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  update adm_Host
  set
   DateModified = GETUTCDATE(), 
   LastUsedLogon = @LoginName
  from
   adm_HostInstance,
   adm_Server2HostMapping
  where
   adm_HostInstance.Name = @Name
   AND adm_Server2HostMapping.Id = adm_HostInstance.Svr2HostMappingId
   AND adm_Host.Id = adm_Server2HostMapping.HostId
  
  update adm_HostInstanceSetting set PropertyValue =
  (CASE
      when PropertyName = N'CLRMaxWorkerThreads' then CAST(@CLRMaxWorkerThreads as nvarchar)
      when PropertyName = N'CLRMinWorkerThreads' then CAST(@CLRMinWorkerThreads as nvarchar)
      when PropertyName = N'CLRMaxIOThreads' then CAST(@CLRMaxIOThreads as nvarchar)
      when PropertyName = N'CLRMinIOThreads' then CAST(@CLRMinIOThreads as nvarchar)
      when PropertyName = N'PhysicalMemoryOptimalUsage' then CAST(@PhysicalMemoryOptimalUsage as nvarchar)
      when PropertyName = N'PhysicalMemoryMaximalUsage' then CAST(@PhysicalMemoryMaximalUsage as nvarchar)
      when PropertyName = N'VirtualMemoryOptimalUsage' then CAST(@VirtualMemoryOptimalUsage as nvarchar)
      when PropertyName = N'VirtualMemoryMaximalUsage' then CAST(@VirtualMemoryMaximalUsage as nvarchar)
   END)
   from
       adm_HostInstance   
   where adm_HostInstance.Name = @Name 
   AND adm_HostInstance.Id = adm_HostInstanceSetting.HostInstanceId
   AND PropertyName IN (N'CLRMaxWorkerThreads',
                        N'CLRMinWorkerThreads',
                        N'CLRMaxIOThreads',
                        N'CLRMinIOThreads',
                        N'PhysicalMemoryOptimalUsage',
                        N'PhysicalMemoryMaximalUsage',
                        N'VirtualMemoryOptimalUsage',
                        N'VirtualMemoryMaximalUsage')                         
exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback transaction
  return @ErrCode
 end
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_HostInstance_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

