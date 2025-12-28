CREATE ROLE [bam_BusinessMessageJournal_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_BusinessMessageJournal_EventWriter', @membername = N'BAM_EVENT_WRITER';

