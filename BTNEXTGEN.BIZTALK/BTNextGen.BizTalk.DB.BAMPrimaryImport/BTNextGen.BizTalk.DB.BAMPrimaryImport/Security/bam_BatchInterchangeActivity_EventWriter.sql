CREATE ROLE [bam_BatchInterchangeActivity_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_BatchInterchangeActivity_EventWriter', @membername = N'BAM_EVENT_WRITER';

