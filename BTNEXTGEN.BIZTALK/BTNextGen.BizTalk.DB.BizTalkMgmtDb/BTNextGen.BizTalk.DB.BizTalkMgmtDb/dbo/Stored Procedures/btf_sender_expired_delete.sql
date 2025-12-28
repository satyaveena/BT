CREATE PROCEDURE [dbo].[btf_sender_expired_delete]
@TExpires 	datetime
AS
    SET NOCOUNT ON
    DELETE FROM [dbo].[btf_message_sender]
    WHERE [expires_at] < @TExpires AND [acknowledged] = 'A'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btf_sender_expired_delete] TO [BTS_HOST_USERS]
    AS [dbo];

