CREATE ROLE [BAM_ManagementNSReader]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'BAM_ManagementNSReader', @membername = N'BWT-Perkins\BizTalk';

