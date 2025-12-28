CREATE PROCEDURE [dbo].[bam_Metadata_GetAuthorizedAccountsForView]
(
    @viewName sysname
)
AS
    SELECT SUSER_SNAME(sid) FROM dbo.sysusers WHERE name = N'dbo'

    SELECT SUSER_SNAME(account.sid) UserName, account.isntgroup IsGroup FROM dbo.sysusers account 
        JOIN dbo.sysmembers member on member.memberuid = account.uid        -- user is in a group
        JOIN dbo.sysusers role ON role.uid = member.groupuid                -- group is the view role
    WHERE (account.isntuser = 1 OR account.isntgroup = 1) AND role.name = N'bam_' + @viewName 
    ORDER BY UserName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetAuthorizedAccountsForView] TO [BAM_ManagementWS]
    AS [dbo];

