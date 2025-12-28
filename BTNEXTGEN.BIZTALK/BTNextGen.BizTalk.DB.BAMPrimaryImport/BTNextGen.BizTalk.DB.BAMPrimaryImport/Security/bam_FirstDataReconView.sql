CREATE ROLE [bam_FirstDataReconView]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_FirstDataReconView', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_FirstDataReconView', @membername = N'BAM_ManagementNSReader';

