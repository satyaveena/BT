CREATE ROLE [BTS_B2B_OPERATORS]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'BTS_B2B_OPERATORS', @membername = N'BWT-Perkins\BizTalk Server B2B Operators';

