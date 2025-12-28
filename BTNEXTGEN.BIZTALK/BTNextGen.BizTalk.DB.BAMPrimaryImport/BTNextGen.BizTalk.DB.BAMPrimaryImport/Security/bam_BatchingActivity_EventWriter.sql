CREATE ROLE [bam_BatchingActivity_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_BatchingActivity_EventWriter', @membername = N'BAM_EVENT_WRITER';

