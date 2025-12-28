CREATE ROLE [bam_EmailTracking]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_EmailTracking', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_EmailTracking', @membername = N'BAM_ManagementNSReader';

