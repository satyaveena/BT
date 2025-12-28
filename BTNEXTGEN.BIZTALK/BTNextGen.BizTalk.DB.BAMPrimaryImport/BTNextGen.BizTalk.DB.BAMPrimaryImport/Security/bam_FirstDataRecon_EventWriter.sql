CREATE ROLE [bam_FirstDataRecon_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_FirstDataRecon_EventWriter', @membername = N'BAM_EVENT_WRITER';

