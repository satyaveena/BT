CREATE ROLE [bam_ExpiredCreditCards]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_ExpiredCreditCards', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_ExpiredCreditCards', @membername = N'BAM_ManagementNSReader';

