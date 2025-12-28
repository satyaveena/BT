CREATE ROLE [bam_Cybersource Settlement Feed_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_Cybersource Settlement Feed_EventWriter', @membername = N'BAM_EVENT_WRITER';

