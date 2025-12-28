CREATE ROLE [BTS_OPERATORS]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'BTS_OPERATORS', @membername = N'BTS_B2B_OPERATORS';


GO
EXECUTE sp_addrolemember @rolename = N'BTS_OPERATORS', @membername = N'BWT-PERKINS\BizTalk Server Operators';

