CREATE PROCEDURE [dbo].[TDDS_BlockTDDS]
AS
begin
	set nocount on
	/*
		This proc must be run in a transaction as the whole idea
		is to block tdds to a caller determined amount of time 
		The time is determined by how long the caller holds the transaction open
	*/
	if @@trancount = 0
	 begin
		return -1
	 end

	select	*
	from 	TDDS_StreamStatus with ( tablockx, holdlock )

	if @@ERROR <> 0
	 BEGIN
		return -1
	 END

	/*
		Success
	*/
	return 0
	
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_BlockTDDS] TO [BTS_BACKUP_USERS]
    AS [dbo];

