CREATE PROCEDURE [dbo].[TDDS_Lock] 
	@resource nvarchar(128),
	@milisecTimeout int,
	@retVal int output
AS
exec @retVal = sp_getapplock @resource,  N'Exclusive', N'Session', @milisecTimeout
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_Lock] TO [BAM_EVENT_WRITER]
    AS [dbo];

