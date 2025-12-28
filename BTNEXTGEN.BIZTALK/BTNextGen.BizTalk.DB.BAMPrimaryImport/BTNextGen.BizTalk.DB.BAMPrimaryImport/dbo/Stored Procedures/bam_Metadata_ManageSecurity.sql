CREATE PROCEDURE [dbo].[bam_Metadata_ManageSecurity]
(
    @roleName            sysname,
    @loginName            sysname,
    @isGrant            BIT
)
AS
    -- Check for view role
    IF NOT EXISTS(SELECT name FROM dbo.sysusers WHERE name =  @roleName AND issqlrole = 1)
    BEGIN
        RAISERROR (N'ManageSecurity_ViewRoleMissing', 16, 1)
        RETURN
    END

    -- Check if account is dbo
    IF EXISTS(SELECT name FROM dbo.sysusers WHERE name = 'dbo' AND SUSER_SNAME(sid) = @loginName) 
    BEGIN
        RAISERROR (N'ManageSecurity_IsDbo', 16, 2)
        RETURN
    END
    
    -- Check if the account is already in the role
    DECLARE @@accountExists INT

    SELECT account.name FROM dbo.sysusers account
        JOIN dbo.sysmembers member ON member.memberuid = account.uid
        JOIN dbo.sysusers role ON role.uid = member.groupuid 
    WHERE SUSER_SNAME(account.sid) = @loginName AND role.name = @roleName

    SET @@accountExists = @@ROWCOUNT
    
    IF @isGrant = 1 AND @@accountExists > 0 -- adding an account already in role, throw
    BEGIN
        RAISERROR (N'ManageSecurity_AccountAlreadyInView', 16, 3)
        RETURN
    END
    ELSE IF @isGrant = 0 AND @@accountExists = 0    -- removing an account not in role, throw
    BEGIN
        RAISERROR (N'ManageSecurity_AccountNotInView', 16, 4)
        RETURN
    END
    
    -- Now actually manage the view permissions
    DECLARE @@userName AS sysname
    SELECT @@userName = name FROM dbo.sysusers WHERE SUSER_SNAME(sid) = @loginName
    
    DECLARE @@retCode INT
    IF @isGrant = 1
    BEGIN
        -- if new user, need to grant db access first
        IF (@@userName IS NULL)
        BEGIN
            EXEC @@retCode = sp_grantdbaccess @loginName, @loginName
            IF (@@retCode <> 0) RETURN 255
                
            SET @@userName = @loginName
        END
        -- ELSE: already has db access

        EXEC sp_addrolemember @roleName, @@userName
        IF (@@ERROR <> 0) RETURN 255
        
        -- Grant permissions on the user object to the BAM_ManagementWS role in case of SQL 2005
        IF (CONVERT(INT, LEFT(CONVERT(CHAR(20), SERVERPROPERTY('ProductVersion')), CHARINDEX('.', CONVERT(CHAR(20), SERVERPROPERTY('ProductVersion'))) - 1)) > 8)
        BEGIN
            DECLARE @grantStatement NVARCHAR(200)
            SET @grantStatement = N'GRANT VIEW DEFINITION ON USER :: "' + @@userName + N'" TO BAM_ManagementWS'
            
            EXEC sp_executesql @grantStatement
        END
        
        IF (@@ERROR <> 0) RETURN 255
    END
    ELSE
    BEGIN
        -- Revoke permissions on the user object to the BAM_ManagementWS role in case of SQL 2005
        IF (CONVERT(INT, LEFT(CONVERT(CHAR(20), SERVERPROPERTY('ProductVersion')), CHARINDEX('.', CONVERT(CHAR(20), SERVERPROPERTY('ProductVersion'))) - 1)) > 8)
        BEGIN
            DECLARE @revokeStatement NVARCHAR(200)
            SET @revokeStatement = N'REVOKE VIEW DEFINITION ON USER :: "' + @@userName + N'" TO BAM_ManagementWS'
            
            EXEC sp_executesql @revokeStatement
        END
    
        EXEC @@retCode = sp_droprolemember @roleName, @@userName
        IF (@@retCode <> 0) RETURN 255
        
        IF NOT EXISTS(SELECT account.name 
                    FROM dbo.sysusers account 
                    JOIN dbo.sysmembers member ON member.memberuid = account.uid
                    JOIN dbo.sysusers role ON role.uid = member.groupuid 
                    WHERE SUSER_SNAME(account.sid) = @loginName)
        BEGIN
            -- If the user is not a member of any other roles, the revoke his DB access
            EXEC @@retCode = sp_revokedbaccess @loginName
            IF (@@retCode <> 0) RETURN 255
        END
    END
    
    RETURN 0