CREATE ROLE [bam_ERPOrders_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_ERPOrders_EventWriter', @membername = N'BAM_EVENT_WRITER';

