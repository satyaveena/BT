CREATE PROCEDURE [dbo].[adpl_Sat_Save]
(
	@ApplicationId [int] ,
	@SdmType [nvarchar] (256) ,
	@Luid [nvarchar] (440) ,
	@Properties [ntext],
	@Files [ntext],
	@CabContent [image] = NULL,
	@Update [Bit],
	@UpdateCab [Bit]
)
AS
SET NOCOUNT ON
DECLARE @ErrCode AS int
DECLARE @IsSystem int
EXEC @IsSystem = dpl_IsSystemAssembly @Name = @Luid
DECLARE @AppId int 
SELECT @AppId = @ApplicationId 
IF ( @IsSystem = 1 )
BEGIN
	SELECT @AppId = nID 
		FROM bts_application
		WHERE isSystem = 1
END
IF (NOT EXISTS(select * from adpl_sat where [luid] = @Luid) OR (@Update = 0))
	BEGIN
		INSERT INTO adpl_sat
		(
		    [applicationId] ,
		    [sdmType],
		    [luid] ,
		    [properties],
		    [files]
		)
		VALUES
		(
		    @AppId,
		    @SdmType,
		    @Luid,
		    @Properties,
		    @Files
		)
	END
ELSE
	BEGIN
		UPDATE adpl_sat
		SET
			[properties] = @Properties,
			[files] = @Files
		WHERE	([luid]  = @Luid)
			AND	([applicationId] = @AppId) 
			AND	([sdmType]  = @SdmType)
	END
	SET @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
	
IF( @UpdateCab = 1 )
BEGIN
	UPDATE adpl_sat
		SET
			[cabContent] = @CabContent
		WHERE	([luid]  = @Luid)
			AND	([applicationId] = @AppId) 
			AND	([sdmType]  = @SdmType)
END
	
RETURN @ErrCode
SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

