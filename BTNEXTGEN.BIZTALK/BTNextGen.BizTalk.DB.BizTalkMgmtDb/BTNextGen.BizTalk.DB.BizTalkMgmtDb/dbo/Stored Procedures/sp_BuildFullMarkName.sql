CREATE PROCEDURE [dbo].[sp_BuildFullMarkName]
@MarkName nvarchar(8), 
@TimeStamp datetime = NULL, 
@UseLocalTime bit=0,
@FullMarkName nvarchar(32) OUTPUT
AS
BEGIN
 declare @date_string nvarchar(128)

 if @TimeStamp is null
 begin
  select @TimeStamp =
   case @UseLocalTime
    when 0 then getutcdate()
    else getdate()
   end
 end
   

 select @date_string = convert( nvarchar, @TimeStamp, 121 )
 select @FullMarkName = @MarkName + N'_' + @date_string
 /*
  Scrub @FullMarkName
 */
 select @FullMarkName = replace( @FullMarkName, ' ', '_' )
 select @FullMarkName = replace( @FullMarkName, '~', '_' )
 select @FullMarkName = replace( @FullMarkName, '!', '_' )
 select @FullMarkName = replace( @FullMarkName, '@', '_' )
 select @FullMarkName = replace( @FullMarkName, '#', '_' )
 select @FullMarkName = replace( @FullMarkName, '$', '_' )
 select @FullMarkName = replace( @FullMarkName, '%', '_' )
 select @FullMarkName = replace( @FullMarkName, '^', '_' )
 select @FullMarkName = replace( @FullMarkName, '&', '_' )
 select @FullMarkName = replace( @FullMarkName, '*', '_' )
 select @FullMarkName = replace( @FullMarkName, '(', '_' )
 select @FullMarkName = replace( @FullMarkName, ')', '_' )
 select @FullMarkName = replace( @FullMarkName, '-', '_' )
 select @FullMarkName = replace( @FullMarkName, '+', '_' )
 select @FullMarkName = replace( @FullMarkName, '+', '_' )
 select @FullMarkName = replace( @FullMarkName, '{', '_' )
 select @FullMarkName = replace( @FullMarkName, '[', '_' )
 select @FullMarkName = replace( @FullMarkName, '}', '_' )
 select @FullMarkName = replace( @FullMarkName, '}', '_' )
 select @FullMarkName = replace( @FullMarkName, '|', '_' )
 select @FullMarkName = replace( @FullMarkName, '\', '_' )
 select @FullMarkName = replace( @FullMarkName, ':', '_' )
 select @FullMarkName = replace( @FullMarkName, ';', '_' )
 select @FullMarkName = replace( @FullMarkName, '''', '' )
 select @FullMarkName = replace( @FullMarkName, '"', '' )
 select @FullMarkName = replace( @FullMarkName, '<', '_' )
 select @FullMarkName = replace( @FullMarkName, '>', '_' )
 select @FullMarkName = replace( @FullMarkName, ',', '_' )
 select @FullMarkName = replace( @FullMarkName, '.', '_' )
 select @FullMarkName = replace( @FullMarkName, '/', '_' )
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_BuildFullMarkName] TO [BTS_BACKUP_USERS]
    AS [dbo];

