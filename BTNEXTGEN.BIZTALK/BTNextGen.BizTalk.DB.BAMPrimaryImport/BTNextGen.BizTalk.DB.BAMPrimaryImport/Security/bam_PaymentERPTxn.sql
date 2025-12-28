CREATE ROLE [bam_PaymentERPTxn]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_PaymentERPTxn', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_PaymentERPTxn', @membername = N'BAM_ManagementNSReader';

