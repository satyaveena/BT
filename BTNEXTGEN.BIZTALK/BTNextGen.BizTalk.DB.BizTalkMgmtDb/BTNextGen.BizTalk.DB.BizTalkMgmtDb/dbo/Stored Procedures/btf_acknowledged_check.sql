CREATE PROCEDURE [dbo].[btf_acknowledged_check]
@TIdentity    varchar(256),
@nCount		  int OUTPUT
AS
    SET NOCOUNT ON
									
	SELECT @nCount = COUNT(*) FROM [dbo].[btf_message_sender] WHERE [identity] = @TIdentity AND [acknowledged] = 'A'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btf_acknowledged_check] TO [BTS_HOST_USERS]
    AS [dbo];

