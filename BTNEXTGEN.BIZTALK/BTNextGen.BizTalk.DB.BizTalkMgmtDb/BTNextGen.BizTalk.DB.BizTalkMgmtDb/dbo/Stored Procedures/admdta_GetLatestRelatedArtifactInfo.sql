CREATE PROCEDURE [dbo].[admdta_GetLatestRelatedArtifactInfo] 
@uidServiceId		uniqueidentifier,
@uidInterceptorId	uniqueidentifier
AS
	SET NOCOUNT ON
	declare @retSvcId uniqueidentifier
	declare @MatchFound int
	set @MatchFound = 0;
	-- First check if artifact is of orchestration type
	IF (EXISTS (SELECT * FROM bts_orchestration WHERE uidGUID = @uidServiceId))
	BEGIN
		SELECT TOP 1 @retSvcId = sti.uidServiceId, @MatchFound=1
		FROM	StaticTrackingInfo sti
		INNER JOIN bts_orchestration o ON o.uidGUID = @uidServiceId
		INNER JOIN bts_assembly a ON a.nID = o.nAssemblyID
		INNER JOIN bts_assembly a2 ON a2.nvcName = a.nvcName AND a2.nvcCulture = a.nvcCulture AND a2.nvcPublicKeyToken = a.nvcPublicKeyToken
		INNER JOIN bts_orchestration o2 ON o2.nAssemblyID = a2.nID
		INNER JOIN TrackinginterceptorVersions tv ON (sti.uidInterceptorId = tv.uidInterceptorID)
		WHERE o2.nvcFullName = o.nvcFullName -- same name but from assembly with different version
				AND o2.uidGUID = sti.uidServiceId
				AND (sti.dtUndeploymentTime is null)
				AND (sti.uidServiceId <> @uidServiceId)
				AND (tv.uidRootInterceptorID = @uidInterceptorId)
		ORDER BY sti.dtDeploymentTime DESC
	END
	ELSE
	BEGIN
		-- if it is not an orchestration, then check if it is a pipeline
		IF (EXISTS (SELECT * FROM bts_item WHERE Guid = @uidServiceId AND IsPipeline = 1))
		BEGIN
			SELECT TOP 1 @retSvcId = sti.uidServiceId, @MatchFound=1
			FROM StaticTrackingInfo sti
			INNER JOIN bts_item i ON i.Guid = @uidServiceId
			INNER JOIN bts_assembly a ON a.nID = i.AssemblyId
			INNER JOIN bts_assembly a2 ON a2.nvcName = a.nvcName AND a2.nvcCulture = a.nvcCulture AND a2.nvcPublicKeyToken = a.nvcPublicKeyToken
			INNER JOIN bts_item i2 ON i2.AssemblyId = a2.nID AND i2.FullName = sti.strServiceName
			INNER JOIN bts_pipeline p ON p.PipelineID = i2.Guid
			INNER JOIN bts_pipeline p2 ON p2.PipelineID = i.Guid
			WHERE p2.Name = p.Name -- same name but from assembly with different version	
					AND i2.Guid = sti.uidServiceId
					AND (sti.dtUndeploymentTime is null)
					AND (sti.uidServiceId <> @uidServiceId)
					AND (sti.uidInterceptorId = @uidInterceptorId)
					AND (p.Category = p2.Category)
			ORDER BY sti.dtDeploymentTime DESC
		END
	END
	if(0 = @MatchFound)
	BEGIN
		-- If related artifact cannot be found, then get the interceptor 
		-- configuration from the group table. There is no service id to be returned, 
		-- so it will be null
		SELECT TOP 1 @uidServiceId, TrackingConfiguration
		FROM adm_Group
	END
	ELSE
	BEGIN
		SELECT	TOP 1 
				sti.uidServiceId, 
				sti.imgData
		FROM	StaticTrackingInfo sti
		INNER JOIN TrackinginterceptorVersions tv ON sti.uidInterceptorId = tv.uidInterceptorID
		WHERE	@retSvcId = sti.uidServiceId
			AND tv.uidRootInterceptorID = @uidInterceptorId
	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_GetLatestRelatedArtifactInfo] TO [BTS_ADMIN_USERS]
    AS [dbo];

