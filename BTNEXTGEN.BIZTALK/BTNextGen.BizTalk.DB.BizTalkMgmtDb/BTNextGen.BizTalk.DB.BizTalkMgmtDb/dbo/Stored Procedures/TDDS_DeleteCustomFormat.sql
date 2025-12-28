create procedure [dbo].[TDDS_DeleteCustomFormat]
(
	@FormatID uniqueidentifier
)
as 
begin
	declare @@ID nvarchar(128)
	select @@ID=CAST ( @FormatID as nvarchar(128))
	if exists(select * from TDDS_CustomFormats where (FormatID = @FormatID) )
	begin
		delete from TDDS_CustomFormats where (FormatID=@FormatID)
		if (@@rowcount =0) 
		begin
		declare @localized_string_error60016 as nvarchar(128)
		set @localized_string_error60016 = N'Can Not Delete Custom Format.'
		raiserror(@localized_string_error60016 ,16,1)
		return 60016
		end
		return 0
	end
	else
	begin
		declare @localized_string_error60017 as nvarchar(128)
		set @localized_string_error60017 = N'Custom Format Does Not Exist.'
		raiserror(@localized_string_error60017, 16, 1)
		return 60017
	end
end
