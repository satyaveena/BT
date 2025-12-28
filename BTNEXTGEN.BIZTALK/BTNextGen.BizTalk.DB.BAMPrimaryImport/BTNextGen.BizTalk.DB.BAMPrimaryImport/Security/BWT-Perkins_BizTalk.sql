CREATE USER [BWT-Perkins\BizTalk] FOR LOGIN [BWT-Perkins\BizTalk]
    WITH DEFAULT_SCHEMA = [BWT-Perkins\BizTalk];


GO
GRANT VIEW DEFINITION
    ON USER::[BWT-Perkins\BizTalk] TO [BAM_ManagementWS]
    AS [BWT-Perkins\BizTalk];

