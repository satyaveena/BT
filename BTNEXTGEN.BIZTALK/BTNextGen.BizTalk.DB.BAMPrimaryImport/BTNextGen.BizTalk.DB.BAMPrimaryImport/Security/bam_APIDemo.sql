CREATE ROLE [bam_APIDemo]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_APIDemo', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_APIDemo', @membername = N'BAM_ManagementNSReader';

