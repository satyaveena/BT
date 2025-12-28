create procedure [dbo].[adm_ReceiveLocation_disable]
@InboundTransportLocation nvarchar(256),
@DisableIt int
as
set nocount on
declare @GroupName nvarchar(256)
begin tran
 --Timestamp table not yet present, add this later
 --select @GroupName = GroupName from adm_ReceiveService WITH (ROWLOCK) where InboundTransportURL = @InboundTransportLocation
UPDATE adm_ReceiveLocation SET Disabled = @DisableIt where InboundTransportURL = @InboundTransportLocation
 --Timestamp table not yet present, add this later
 --Update corresponding record in the TimeStamp table
 --update adm_TimeStamps set ReceiveServiceLastTimeStamp = getDate() where GroupName = @GroupName
commit tran
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ReceiveLocation_disable] TO [BTS_HOST_USERS]
    AS [dbo];

