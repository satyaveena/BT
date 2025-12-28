create procedure [dbo].[TDDS_CleanService]
(
	@ServiceID uniqueidentifier
)
as 
begin
	delete from TDDS_Heartbeats where ServiceID=@ServiceID
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_CleanService] TO [BAM_CONFIG_READER]
    AS [dbo];

