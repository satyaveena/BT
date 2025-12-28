CREATE ROLE [BAM_CONFIG_READER]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'BAM_CONFIG_READER', @membername = N'BTS_HOST_USERS';

