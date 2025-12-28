CREATE ROLE [bam_FunctionalAckActivity_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_FunctionalAckActivity_EventWriter', @membername = N'BAM_EVENT_WRITER';

