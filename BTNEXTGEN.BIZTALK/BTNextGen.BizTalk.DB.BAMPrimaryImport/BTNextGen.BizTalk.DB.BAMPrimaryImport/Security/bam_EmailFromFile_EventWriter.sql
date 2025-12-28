CREATE ROLE [bam_EmailFromFile_EventWriter]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_EmailFromFile_EventWriter', @membername = N'BAM_EVENT_WRITER';

