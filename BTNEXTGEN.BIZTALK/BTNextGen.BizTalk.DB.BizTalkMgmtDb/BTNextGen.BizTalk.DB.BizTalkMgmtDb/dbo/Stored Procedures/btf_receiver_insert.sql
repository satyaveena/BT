CREATE PROCEDURE [dbo].[btf_receiver_insert]
@TIdentity    varchar(256),
@TLifespan    datetime,
@fSuccess	  int OUTPUT
AS
    SET NOCOUNT ON
    DECLARE @nodes AS INTEGER
    INSERT INTO [dbo].[btf_message_receiver]  ( [identity], [expires_at] )
									VALUES    ( @TIdentity, @TLifespan )
    set @fSuccess = @@ROWCOUNT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btf_receiver_insert] TO [BTS_HOST_USERS]
    AS [dbo];

