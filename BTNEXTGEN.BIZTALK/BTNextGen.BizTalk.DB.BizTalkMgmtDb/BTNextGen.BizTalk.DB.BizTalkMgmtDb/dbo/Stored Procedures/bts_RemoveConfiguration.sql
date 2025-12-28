CREATE PROCEDURE [dbo].[bts_RemoveConfiguration]
@strAssemblyFullName	nvarchar(256)
AS
	declare @dtUndeploymentTime datetime
	set @dtUndeploymentTime = GETUTCDATE()
	-- mark all schedules that belong to this assembly
	UPDATE	StaticTrackingInfo
	SET		dtUndeploymentTime = @dtUndeploymentTime
	FROM 	bts_orchestration o, 
			bts_assembly a
	WHERE	o.nAssemblyID = a.nID
			AND o.uidGUID = uidServiceId
			AND a.nvcFullName = @strAssemblyFullName
	
	-- mark all pipelines that belong to this assembly
	UPDATE	StaticTrackingInfo
	SET		dtUndeploymentTime = @dtUndeploymentTime
	FROM 	bts_item i, 
			bts_assembly a
	WHERE	i.AssemblyId = a.nID
			AND i.Guid = uidServiceId
			AND a.nvcFullName = @strAssemblyFullName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_RemoveConfiguration] TO [BTS_ADMIN_USERS]
    AS [dbo];

