CREATE ROLE [bam_InterchangeStatusActivity_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_InterchangeStatusActivity_EventWriter', @membername = N'BAM_EVENT_WRITER';

