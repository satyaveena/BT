CREATE ROLE [bam_active_CCards_view]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_active_CCards_view', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_active_CCards_view', @membername = N'BAM_ManagementNSReader';

