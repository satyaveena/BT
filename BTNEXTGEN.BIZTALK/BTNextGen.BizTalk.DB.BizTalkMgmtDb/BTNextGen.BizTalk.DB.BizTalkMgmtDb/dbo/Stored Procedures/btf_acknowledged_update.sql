CREATE PROCEDURE [dbo].[btf_acknowledged_update]
@TIdentity    varchar(256),
@dtExpires	  datetime
AS
    SET NOCOUNT ON
    INSERT INTO [dbo].[btf_message_sender] ([identity], [expires_at], [acknowledged] )
									VALUES (@TIdentity, @dtExpires,   'A')

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btf_acknowledged_update] TO [BTS_HOST_USERS]
    AS [dbo];

