-- Get RTA mutex stored proc
CREATE PROCEDURE [dbo].[bam_Metadata_GetRTAMutex]
AS
    DECLARE @@par INT
    SELECT @@par = partitionID FROM [dbo].[bam_Metadata_RTAMutex] WITH (XLOCK, ROWLOCK) WHERE partitionID = @@spid % 10
    RETURN @@par    -- Do not remove return code, this is used by RTA trigger