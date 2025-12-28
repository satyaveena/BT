CREATE ROLE [BTS_ADMIN_USERS]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'BTS_ADMIN_USERS', @membername = N'BWT-PERKINS\BizTalk Server Administrators';

