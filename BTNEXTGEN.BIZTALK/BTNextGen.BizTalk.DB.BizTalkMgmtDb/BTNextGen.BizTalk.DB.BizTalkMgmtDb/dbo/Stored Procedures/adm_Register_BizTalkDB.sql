CREATE PROCEDURE [dbo].[adm_Register_BizTalkDB]
@BizTalkDBName nvarchar(64),
@DatabaseMajor int,
@DatabaseMinor int,
@DatabaseBuildNumber int,
@DatabaseRevision int,
@ProductMajor int,
@ProductMinor int,
@ProductBuildNumber int,
@ProductRevision int,
@ProductLanguage nvarchar(256),
@Description nvarchar(256)
AS
	set nocount on
	set xact_abort on
	begin transaction
		-- Note: This will fail if there already exist an entry with the same BizTalkDBName
		insert into BizTalkDBVersion
		(
			BizTalkDBName,
			DatabaseMajor,
			DatabaseMinor,
			DatabaseBuildNumber,
			DatabaseRevision,
			ProductMajor,	
			ProductMinor,	
			ProductBuildNumber,
			ProductRevision,
			ProductLanguage,
			Description
		)
		values
		(
			@BizTalkDBName,
			@DatabaseMajor,
			@DatabaseMinor,
			@DatabaseBuildNumber,
			@DatabaseRevision,
			@ProductMajor,	
			@ProductMinor,	
			@ProductBuildNumber,
			@ProductRevision,
			@ProductLanguage,
			@Description
		)
		
	commit transaction
	set nocount off
