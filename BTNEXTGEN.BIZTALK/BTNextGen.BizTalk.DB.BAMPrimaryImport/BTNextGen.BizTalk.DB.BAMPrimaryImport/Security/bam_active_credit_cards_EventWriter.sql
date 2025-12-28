CREATE ROLE [bam_active_credit_cards_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_active_credit_cards_EventWriter', @membername = N'BAM_EVENT_WRITER';

