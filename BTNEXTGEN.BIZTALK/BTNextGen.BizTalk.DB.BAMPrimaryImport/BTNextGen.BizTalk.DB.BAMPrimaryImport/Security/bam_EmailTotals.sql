CREATE ROLE [bam_EmailTotals]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_EmailTotals', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_EmailTotals', @membername = N'BAM_ManagementNSReader';

