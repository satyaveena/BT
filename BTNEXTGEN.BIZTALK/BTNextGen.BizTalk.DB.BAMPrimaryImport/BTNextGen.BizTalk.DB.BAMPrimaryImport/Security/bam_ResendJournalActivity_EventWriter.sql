CREATE ROLE [bam_ResendJournalActivity_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_ResendJournalActivity_EventWriter', @membername = N'BAM_EVENT_WRITER';

