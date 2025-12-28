CREATE ROLE [bam_CyberSourceFeedV]
    AUTHORIZATION [db_owner];


GO
EXECUTE sp_addrolemember @rolename = N'bam_CyberSourceFeedV', @membername = N'BAM_ManagementWS';


GO
EXECUTE sp_addrolemember @rolename = N'bam_CyberSourceFeedV', @membername = N'BAM_ManagementNSReader';

