CREATE ROLE [BTS_HOST_USERS]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'BTS_HOST_USERS', @membername = N'BWT-PERKINS\BizTalk Application Users';


GO
EXECUTE sp_addrolemember @rolename = N'BTS_HOST_USERS', @membername = N'BWT-PERKINS\BizTalk Isolated Host Users';

