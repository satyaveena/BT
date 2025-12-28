CREATE ROLE [bam_ExpiredCreditCards_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_ExpiredCreditCards_EventWriter', @membername = N'BAM_EVENT_WRITER';

