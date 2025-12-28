CREATE PROCEDURE [dbo].[bts_GetGroupUUID]
 @nGroupUUID uniqueidentifier output
AS
 select top 1 @nGroupUUID = UUID from [dbo].[adm_Group] with (NOLOCK)
 if (@@ROWCOUNT <> 1)
  return -1
 return 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetGroupUUID] TO [BTS_HOST_USERS]
    AS [dbo];

