CREATE PROCEDURE [dbo].[adpl_Sat_Property_Update]
(
      @Luid [nvarchar] (440) ,
      @Properties [ntext]
)
AS
SET NOCOUNT ON
declare @ErrCode as int
UPDATE adpl_sat
SET [properties] = @Properties
WHERE ([luid]  = @Luid)
set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
return @ErrCode
SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Property_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

