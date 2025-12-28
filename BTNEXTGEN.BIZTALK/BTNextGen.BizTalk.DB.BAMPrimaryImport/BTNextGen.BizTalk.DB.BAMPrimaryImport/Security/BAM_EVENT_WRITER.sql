CREATE ROLE [BAM_EVENT_WRITER]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'BAM_EVENT_WRITER', @membername = N'BWT-PERKINS\BizTalk Application Users';

