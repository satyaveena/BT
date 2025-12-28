create procedure [dbo].[TDDS_CreateCustomFormat]
(
	@FormatID uniqueidentifier,
	@DecoderClass nvarchar(256),
	@DLLName nvarchar(1024)
)
as
begin
	declare @@ID nvarchar(128)
	select @@ID=CAST ( @FormatID as nvarchar(128))
	if (exists(select * from TDDS_CustomFormats where FormatID=@FormatID))
	begin
		declare @localized_string_error60014 as nvarchar(128)
		set @localized_string_error60014 = N'Custom Format Already Exists.'
		raiserror (@localized_string_error60014,16,1)
		return 60014
	end
	else
	begin
		insert TDDS_CustomFormats (FormatID,DecoderClass,DllName)
		values(@FormatID,@DecoderClass,@DLLName)
		if (@@rowcount =0) 
		begin
			declare @localized_string_error60015 as nvarchar(128)
			set @localized_string_error60015 = N'Can Not Create Custom Format.'
			raiserror(@localized_string_error60015 , 16,1)
			return 60015
		end
		return 0
	end
end
