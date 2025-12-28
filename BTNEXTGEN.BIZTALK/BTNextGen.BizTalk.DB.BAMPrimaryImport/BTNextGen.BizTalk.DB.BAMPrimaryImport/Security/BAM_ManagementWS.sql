CREATE ROLE [BAM_ManagementWS]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'BAM_ManagementWS', @membername = N'BWT-Perkins\BizTalk';

