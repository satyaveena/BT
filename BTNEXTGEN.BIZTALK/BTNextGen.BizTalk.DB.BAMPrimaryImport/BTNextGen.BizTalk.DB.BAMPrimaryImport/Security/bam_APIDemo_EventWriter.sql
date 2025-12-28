CREATE ROLE [bam_APIDemo_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_APIDemo_EventWriter', @membername = N'BAM_EVENT_WRITER';

