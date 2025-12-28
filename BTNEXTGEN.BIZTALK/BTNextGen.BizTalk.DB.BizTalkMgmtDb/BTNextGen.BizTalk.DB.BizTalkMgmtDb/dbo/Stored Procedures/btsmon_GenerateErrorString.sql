CREATE PROCEDURE [dbo].[btsmon_GenerateErrorString]
AS
 declare @count bigint
 declare @Server sysname
 declare @Name sysname
 declare @ProblemDescription nvarchar(128)
 declare @errorString nvarchar(MAX)
 declare @error bit
 
 declare @localised_string_in nvarchar(4)
 set @localised_string_in = N' in '

 set @errorString = N''
 declare Inconsistancy_Cursor insensitive cursor for
 SELECT inc.DBServer, inc.DBName, iss.nvcProblemDescription, inc.nCount FROM [dbo].[btsmon_Inconsistancies] inc JOIN [dbo].[btsmon_Issues] iss on iss.[nProblemCode] = inc.[nProblemCode] order by inc.nCount DESC

 set @error = 0
 open Inconsistancy_Cursor
 fetch next from Inconsistancy_Cursor into @Server, @Name, @ProblemDescription, @count
 while @@fetch_status = 0
 begin
  if (@count > 0)
  begin
   if (@error = 1) set @errorString = @errorString + N', '
   set @errorString = @errorString + CONVERT(nvarchar(20), @count) + N' ' + @ProblemDescription + @localised_string_in + @Server + N'.' + @Name
   set @error = 1 
  end
  fetch next from Inconsistancy_Cursor into @Server, @Name, @ProblemDescription, @count
 end
 
 if (@error = 1) RAISERROR(@errorString, 11, 1) WITH LOG
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btsmon_GenerateErrorString] TO [BTS_ADMIN_USERS]
    AS [dbo];

