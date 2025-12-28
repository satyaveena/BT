CREATE ROLE [bam_FunctionalGroupInfo_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_FunctionalGroupInfo_EventWriter', @membername = N'BAM_EVENT_WRITER';

