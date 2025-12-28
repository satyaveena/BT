CREATE PROCEDURE [dbo].[sp_SetMarkRemote]
 @name nvarchar (128)
  AS
BEGIN
 BEGIN TRANSACTION @name WITH MARK @name

  INSERT INTO [dbo].[MarkLog]([MarkName])
  VALUES(@name)

  IF @@Error <> 0
   BEGIN
   GOTO FAILED
   END

 COMMIT TRANSACTION @name

 RETURN 0

FAILED:
 if @@trancount > 0
  rollback transaction @name
   
 return -1
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_SetMarkRemote] TO [BTS_BACKUP_USERS]
    AS [dbo];

