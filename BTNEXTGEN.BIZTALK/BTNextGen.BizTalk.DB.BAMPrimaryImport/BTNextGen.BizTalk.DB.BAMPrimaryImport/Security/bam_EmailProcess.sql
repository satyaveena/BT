CREATE ROLE [bam_EmailProcess]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_EmailProcess', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_EmailProcess', @membername = N'BAM_ManagementNSReader';

