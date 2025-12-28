-- Find Stored Procedures based on cubeID

CREATE PROCEDURE [dbo].[TDDS_GetSPNames]
 AS
SELECT u.name + '.' + o.name FROM sysobjects o
JOIN sysusers u ON  u.uid = o.uid
WHERE o.type = 'P'
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetSPNames] TO [BAM_EVENT_WRITER]
    AS [dbo];

