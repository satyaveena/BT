CREATE ROLE [bam_ERPOrders]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_ERPOrders', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_ERPOrders', @membername = N'BAM_ManagementNSReader';

