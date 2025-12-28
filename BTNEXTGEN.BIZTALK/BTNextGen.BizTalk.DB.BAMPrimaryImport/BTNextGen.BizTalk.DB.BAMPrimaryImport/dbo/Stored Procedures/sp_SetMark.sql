CREATE PROCEDURE [dbo].[sp_SetMark]
 @name nvarchar (128)
  AS
BEGIN
 BEGIN TRANSACTION @name 

  INSERT INTO [dbo].[MarkLog]([MarkName])
  VALUES(@name)

  IF @@Error <> 0
   BEGIN
   GOTO FAILED
   END

 COMMIT TRANSACTION @name

 return 0

FAILED:
 if @@trancount > 0
  rollback transaction @name

 return -1
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_SetMark] TO [BTS_BACKUP_USERS]
    AS [dbo];

