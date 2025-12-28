CREATE PROCEDURE [dbo].[admdta_GetServices]
@strGroupName	nvarchar(256),
@serviceType	int		--ServiceTypes.XLangs = 0, ServiceTypes.Pipeline = 1
AS
	-- We need the interceptor id because we do not want to show other interceptor
	-- configurations in HAT
	if(0 = @serviceType) 
		begin
			SELECT	o.uidGUID as ServiceId, 
			 		o.uidGUID as VersionIndependentId,
					sti.uidInterceptorId as InterceptorId,
					sti.strServiceName as ServiceName,
					0 as ServiceType, -- 0 stands for XLANGs services
					a.nvcName as AssemblyName,
					sti.imgData as InterceptorConfiguration,
					a.nvcDescription as N'Description',
					0 as ServiceSubType,	-- 0 meaning N/A for XLANGs services
					a.nvcPublicKeyToken as PublicKeyToken,
					a.nvcVersion as Version,
					a.nvcCulture as Culture,
					tv.uidRootInterceptorID as RootInterceptorId
			FROM	bts_orchestration o 
			INNER JOIN bts_assembly a ON a.nID = o.nAssemblyID
			INNER JOIN StaticTrackingInfo sti ON sti.uidServiceId = o.uidGUID
			INNER JOIN TrackinginterceptorVersions tv ON tv.uidRootInterceptorID = N'{1E83A7DC-435E-49DF-BA83-F09CA50DFBE7}' -- XLang/s native interceptor
			WHERE sti.uidInterceptorId = tv.uidInterceptorID
		end
	else
		begin
			SELECT	i.Guid as ServiceId,
					i.Guid as VersionIndependentId,
					sti.uidInterceptorId as InterceptorId,
					sti.strServiceName as ServiceName,
					1 as ServiceType, -- 1 stands for pipeline
					a.nvcName as AssemblyName,
					sti.imgData as InterceptorConfiguration,
					a.nvcDescription as N'Description',
					cast (p.Category as int) as ServiceSubType, -- 1 - Receive & 2 - Transmit pipelines
					a.nvcPublicKeyToken as PublicKeyToken,
					a.nvcVersion as Version,
					a.nvcCulture as Culture,
					sti.uidInterceptorId as RootInterceptorId
			FROM bts_item i
			INNER JOIN bts_assembly a ON a.nID = i.AssemblyId 
			INNER JOIN StaticTrackingInfo sti ON sti.uidServiceId = i.Guid
			INNER JOIN bts_pipeline p ON p.PipelineID = i.Guid
			WHERE	i.IsPipeline = 1
					AND sti.uidInterceptorId = N'{D90B63BA-3EEB-4E8A-A20E-7BE683319552}' -- Messaging Interceptor
		end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_GetServices] TO [BTS_ADMIN_USERS]
    AS [dbo];

