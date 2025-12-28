CREATE FUNCTION [dbo].[adm_GetNumInstalledHostInstances] (@HostName nvarchar(80))
RETURNS int
AS
BEGIN
    return
        (select count(*)
         from adm_Host host, adm_Server2HostMapping mapping, adm_HostInstance inst
         where host.Name = @HostName
            AND host.Id = mapping.HostId
            AND mapping.Id = inst.Svr2HostMappingId
            AND inst.ConfigurationState = 1   -- counting only the INSTALLED ones
        )
END