CREATE ROLE [bam_InterchangeAckActivity_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_InterchangeAckActivity_EventWriter', @membername = N'BAM_EVENT_WRITER';

