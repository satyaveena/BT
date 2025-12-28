CREATE FUNCTION [dbo].[edi_SplitString]
(
	@List nvarchar(2000),
	@Delimiter nvarchar(5)
)  
RETURNS @SplitValue table 
(		
	Id int identity(1,1),
	Value nvarchar(256)
) 
AS  
BEGIN

While (Charindex(@Delimiter,@List)>0)
Begin 
Insert Into @SplitValue (Value)
Select 
    Value = ltrim(rtrim(Substring(@List,1,Charindex(@Delimiter,@List)-1))) 
	Set @List = Substring(@List,Charindex(@Delimiter,@List)+len(@Delimiter),len(@List))
End 
	Insert Into @SplitValue (Value)
    Select Value = ltrim(rtrim(@List))

    Return

END
GO
GRANT SELECT
    ON OBJECT::[dbo].[edi_SplitString] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[edi_SplitString] TO [BTS_B2B_OPERATORS]
    AS [dbo];

