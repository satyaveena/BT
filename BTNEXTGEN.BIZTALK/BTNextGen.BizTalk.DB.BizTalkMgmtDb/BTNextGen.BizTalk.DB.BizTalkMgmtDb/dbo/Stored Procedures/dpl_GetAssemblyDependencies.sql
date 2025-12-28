CREATE PROCEDURE [dbo].[dpl_GetAssemblyDependencies]
(
 @ModuleId int
)

AS
 /* SET NOCOUNT ON */
 
 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[#dependencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
  DROP TABLE [dbo].[#dependencies]   
 END
 
 CREATE TABLE [dbo].#dependencies (
   [nReferredAssemblyID] [int] NOT NULL 
 )
 
 /* Insert direct dependencies */
 INSERT INTO #dependencies
 SELECT idlib 
  FROM bts_libreference 
  WHERE idapp = @ModuleId
  

 DECLARE @rowsadded int
 SELECT @rowsadded = 1 /* Let the loop run for the first time */
 
 /* Now repeat inserting all dependencies of modules already in dependencies
  until nothing more can be inserted
  */ 
 WHILE @rowsadded > 0
  BEGIN
   INSERT INTO #dependencies
   SELECT idlib 
    FROM bts_libreference 
    WHERE idapp IN ( SELECT nReferredAssemblyID FROM #dependencies )
     and idlib NOT IN ( SELECT nReferredAssemblyID FROM #dependencies )
   
   SELECT @rowsadded = @@ROWCOUNT
  END
 
  
 SELECT DISTINCT b.nId, b.nvcName, b.nVersionMajor, b.nVersionMinor, b.nVersionBuild, b.nVersionRevision
   FROM #dependencies d 
   INNER JOIN bts_assembly b ON b.nId=d.nReferredAssemblyID
   ORDER BY nvcName, nVersionMajor, nVersionMinor, nVersionBuild, nVersionRevision
 
 /* If module passed as a parameter showed up as its own dependency, this means it is the part 
  of the circular referencing loop
  - in this case stored proc will return value > 0 */
 RETURN (SELECT COUNT(*) FROM #dependencies WHERE nReferredAssemblyID=@ModuleId)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetAssemblyDependencies] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetAssemblyDependencies] TO [BTS_OPERATORS]
    AS [dbo];

