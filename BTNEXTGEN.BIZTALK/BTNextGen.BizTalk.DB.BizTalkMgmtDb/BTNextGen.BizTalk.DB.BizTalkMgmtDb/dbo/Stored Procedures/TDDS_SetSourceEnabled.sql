create procedure [dbo].[TDDS_SetSourceEnabled]
(
	@SourceName	nvarchar(256),
	@Enable	        bit
)
as
begin
	
	if (@Enable is null)
	begin
			declare @localized_string_error61000 as nvarchar(128)
			set @localized_string_error61000 = N'Enabled cannot be set to null.'
			raiserror(@localized_string_error61000, 16, 1)
			return 61000
	end
	else
	begin
	
		if exists(select * from TDDS_Sources where  (SourceName=@SourceName))
		begin
			update TDDS_Sources with (TABLOCK)
			set Enabled = @Enable
			where (SourceName=@SourceName)
			
		end
		else
		begin
			declare @localized_string_error62011 as nvarchar(128)
			set @localized_string_error62011 = N'Source Does Not Exist.'
			raiserror(@localized_string_error62011, 16, 1)
			return 62011
		end
	end
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_SetSourceEnabled] TO [BTS_ADMIN_USERS]
    AS [dbo];

