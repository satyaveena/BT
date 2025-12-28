CREATE ROLE [bam_PaymentERPTxn_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_PaymentERPTxn_EventWriter', @membername = N'BAM_EVENT_WRITER';

