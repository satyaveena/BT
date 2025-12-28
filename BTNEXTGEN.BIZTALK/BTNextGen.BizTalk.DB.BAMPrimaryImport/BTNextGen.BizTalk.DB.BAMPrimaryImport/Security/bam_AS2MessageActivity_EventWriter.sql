CREATE ROLE [bam_AS2MessageActivity_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_AS2MessageActivity_EventWriter', @membername = N'BAM_EVENT_WRITER';

