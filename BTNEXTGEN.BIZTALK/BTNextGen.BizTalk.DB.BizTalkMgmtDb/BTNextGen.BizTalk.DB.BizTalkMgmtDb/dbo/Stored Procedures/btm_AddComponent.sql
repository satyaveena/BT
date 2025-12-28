CREATE PROCEDURE [dbo].[btm_AddComponent]
@StageID int,
@Sequence int,
@Name nvarchar(64),
@Version nvarchar(10),
@ClsID uniqueidentifier=null,
@TypeName nvarchar(256),
@AssemblyPath nvarchar(256),
@Description nvarchar(256),
@CustomData image,
@CompID int OUTPUT
AS
INSERT INTO bts_component (Name, Version, ClsID, TypeName, AssemblyPath, Description, CustomData)
		 VALUES (@Name, @Version, @ClsID, @TypeName, @AssemblyPath, @Description, @CustomData)
select @CompID = @@IDENTITY
INSERT INTO bts_stage_config ( StageID, CompID, Sequence)
	   VALUES ( @StageID, @CompID, @Sequence)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btm_AddComponent] TO [BTS_ADMIN_USERS]
    AS [dbo];

