CREATE PROCEDURE [dbo].[btf_receiver_expired_delete]
@TExpires 	datetime
AS
    SET NOCOUNT ON
    DELETE FROM [dbo].[btf_message_receiver]
    WHERE [expires_at] < @TExpires

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btf_receiver_expired_delete] TO [BTS_HOST_USERS]
    AS [dbo];

