CREATE PROCEDURE [dbo].[btm_CreatePipeline]
@PipelineID uniqueidentifier,
@Category smallint,
@Name nvarchar(256),
@FullyQualifiedName nvarchar(256),
@IsStreaming smallint,
@AssemblyID int,
@Release int,
@ID int OUTPUT
AS
set xact_abort on
INSERT INTO bts_pipeline(PipelineID, Category, Name, FullyQualifiedName, IsStreaming, nAssemblyID, Release)
		VALUES (@PipelineID, @Category, @Name, @FullyQualifiedName, @IsStreaming, @AssemblyID, @Release)
		
set @ID = @@IDENTITY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btm_CreatePipeline] TO [BTS_ADMIN_USERS]
    AS [dbo];

