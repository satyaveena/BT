CREATE PROCEDURE [dbo].[dpl_VerifyAssemblyReferences]
 @Name nvarchar(256),
 @VersionMajor int,
 @VersionMinor int,
 @VersionBuild int,
 @VersionRevision int,
 @Culture nvarchar(25),
 @PublicKeyToken nvarchar(256)

AS
 DECLARE @ID int
 DECLARE @ret int

 DECLARE @rName nvarchar(256)
 DECLARE @rVersion nvarchar(12)
 DECLARE @rMinorVersion nvarchar(12)
 DECLARE @rBuildVersion nvarchar(12)
 DECLARE @rRevisionVersion nvarchar(12)
 DECLARE @rArtifactName nvarchar(256)
 DECLARE @rCulture nvarchar(256)
 DECLARE @rPublicKeyToken nvarchar(256)

 DECLARE @refName nvarchar(256)
 DECLARE @refType int
 DECLARE @refMajor int
 DECLARE @refMinor int
 DECLARE @refBuild int
 DECLARE @refRevision int
 DECLARE @refCulture nvarchar(256)
 DECLARE @refPublicKeyToken nvarchar(256)
 
 SELECT
  @ID = nID,
  @refName = nvcName,
  @refType = nType,
  @refMajor = nVersionMajor,
  @refMinor = nVersionMinor,
  @refBuild = nVersionBuild,
  @refRevision = nVersionRevision,
  @refCulture = nvcCulture,
  @refPublicKeyToken = nvcPublicKeyToken
 FROM bts_assembly
 WHERE
  (@Name = nvcName) and
  (@VersionMajor = nVersionMajor) and
  (@VersionMinor = nVersionMinor) and
  (@VersionBuild = nVersionBuild) and
  (@VersionRevision = nVersionRevision) and
  (@PublicKeyToken = nvcPublicKeyToken) and
  (nType > 0)
    
 IF @@ROWCOUNT = 0
  RETURN -1
    
 DECLARE refs_cur CURSOR LOCAL FAST_FORWARD FOR
 SELECT nvcAssemblyName, nvcVersionMajor, nvcVersionMinor, nvcVersionBuild, nvcVersionRevision, nvcItemName, nvcCulture, nvcPublicKeyToken
 FROM bts_itemreference
 WHERE (@ID = nReferringAssemblyID)
 OPEN refs_cur
 
 FETCH NEXT FROM refs_cur INTO @rName, @rVersion, @rMinorVersion, @rBuildVersion, @rRevisionVersion, @rArtifactName, @rCulture, @rPublicKeyToken 
 IF @@FETCH_STATUS <> 0 
  BEGIN
   CLOSE refs_cur
   DEALLOCATE refs_cur
   RETURN 0 /* OK, no references */
  END
 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[#badreferences]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
  DROP TABLE [dbo].[#badreferences]   
 END
 CREATE TABLE [dbo].[#badreferences] (
  [nReferringModuleID] [int] NOT NULL ,
  [nReferringModuleName] [nvarchar](256) NOT NULL ,
  [nReferringModuleType] [tinyint] NOT NULL ,
  [nReferringModuleMajor] [smallint] NOT NULL ,
  [nReferringModuleMinor] [smallint] NOT NULL ,
  [nReferringModuleBuild] [smallint] NOT NULL ,
  [nReferringModuleRevision] [smallint] NOT NULL ,
  [nErrorCode] [int] NOT NULL,
  [nvcModuleName] [nvarchar] (256) NOT NULL ,
  [nvcProductMajorVersion] [nvarchar] (10) NOT NULL ,
  [nvcProductMinorVersion] [nvarchar] (10) NOT NULL ,
  [nvcProductBuildVersion] [nvarchar] (10) NOT NULL ,
  [nvcProductRevisionVersion] [nvarchar] (10) NOT NULL ,
  [nvcArtifactName] [nvarchar] (256) NOT NULL 
 ) 
 WHILE @@FETCH_STATUS = 0
 BEGIN
  -- forward reference: dbo.dpl_VerifyReference
  EXECUTE @ret = dbo.dpl_VerifyReference  @Name = @rName ,
            @Type = '2' ,
            @VersionMajor = @rVersion ,
            @VersionMinor = @rMinorVersion ,
            @VersionBuild = @rBuildVersion,
            @VersionRevision = @rRevisionVersion,
            @ArtifactName = @rArtifactName ,
            @ArtifactType = '',
            @Culture = @rCulture,
            @PublicKeyToken = @rPublicKeyToken
  IF @ret <> 0
   INSERT INTO [#badreferences]( nReferringModuleID, 
           nReferringModuleName, 
           nReferringModuleType, 
           nReferringModuleMajor, 
           nReferringModuleMinor, 
           nReferringModuleBuild, 
           nReferringModuleRevision, 
           nErrorCode,
           nvcModuleName,
           nvcProductMajorVersion,
           nvcProductMinorVersion,
           nvcProductBuildVersion,
           nvcProductRevisionVersion,
           nvcArtifactName )
      VALUES( @ID,
        @refName,
        @refType,
        @refMajor,
        @refMinor,
        @refBuild,
        @refRevision,
        @ret,
        @rName,
        @rVersion,
        @rMinorVersion,
        @rBuildVersion,
        @rRevisionVersion,
        @rArtifactName )
  FETCH NEXT FROM refs_cur INTO @rName, @rVersion, @rMinorVersion, @rBuildVersion, @rRevisionVersion, @rArtifactName, @rCulture, @rPublicKeyToken 
 END
 
 CLOSE refs_cur
 DEALLOCATE refs_cur
 
 SELECT nReferringModuleID,   --0
   nReferringModuleName,  --1
   nReferringModuleType,  --2
   nReferringModuleMajor,  --3
   nReferringModuleMinor,  --4
   nReferringModuleBuild,  --5
   nReferringModuleRevision, --6
   nErrorCode,     --7
   nvcModuleName,    --8
   nvcProductMajorVersion,  --9
   nvcProductMinorVersion,  --10
   nvcProductBuildVersion,  --11
   nvcProductRevisionVersion, --12
   nvcArtifactName    --13
   FROM [dbo].[#badreferences]
   

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_VerifyAssemblyReferences] TO [BTS_ADMIN_USERS]
    AS [dbo];

