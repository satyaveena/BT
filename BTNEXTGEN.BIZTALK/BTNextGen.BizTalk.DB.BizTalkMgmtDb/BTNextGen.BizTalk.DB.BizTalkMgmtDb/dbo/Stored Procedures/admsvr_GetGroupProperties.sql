CREATE PROCEDURE [dbo].[admsvr_GetGroupProperties]
	@nvcGroupName			nvarchar(256), -- No longer used
	@nvcHostName			nvarchar(256),
	@nCacheRefreshInterval	int OUTPUT,
	@nLMSFragmentSize		int	OUTPUT,
	@nLMSThreshold			int OUTPUT,
             @nThreadPoolSize			int OUTPUT
AS
	SELECT TOP 1
		@nCacheRefreshInterval	= adm_Group.ConfigurationCacheRefreshInterval,
		@nLMSFragmentSize		= adm_Group.LMSFragmentSize,
		@nLMSThreshold			= adm_Group.LMSThreshold
	FROM 
		adm_Group
        SELECT 
		@nThreadPoolSize = adm_Host.ThreadPoolSize
	FROM  
		adm_Host
	WHERE
		adm_Host.Name = @nvcHostName

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetGroupProperties] TO [BTS_HOST_USERS]
    AS [dbo];

