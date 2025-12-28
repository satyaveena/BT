CREATE PROCEDURE [dbo].[dpl_RecalculateAssemblyReferences]

AS


DELETE FROM  bts_libreference

INSERT INTO bts_libreference(idapp,idlib,refName)
SELECT DISTINCT nReferringAssemblyID AS appId, 
 CASE WHEN
 ( SELECT nID 
  FROM bts_assembly bm
  WHERE bm.nvcName = bar.nvcAssemblyName and 
     2 = bm.nType and
     bm.nVersionMajor = bar.nvcVersionMajor and 
    bm.nVersionMinor = bar.nvcVersionMinor and
    bm.nVersionBuild = bar.nvcVersionBuild and
    bm.nVersionRevision = bar.nvcVersionRevision and
    bm.nvcPublicKeyToken = bar.nvcPublicKeyToken 
 ) IS NOT NULL THEN
   ( SELECT nID 
  FROM bts_assembly bm
  WHERE bm.nvcName = bar.nvcAssemblyName and 
     2 = bm.nType and
     bm.nVersionMajor = bar.nvcVersionMajor and 
    bm.nVersionMinor = bar.nvcVersionMinor and
    bm.nVersionBuild = bar.nvcVersionBuild and
    bm.nVersionRevision = bar.nvcVersionRevision and
    bm.nvcPublicKeyToken = bar.nvcPublicKeyToken 
  )
 ELSE 
  ( SELECT TOP 1 bm.nID 
   FROM bts_assembly bm
   WHERE nSystemAssembly = 1
   ORDER BY bm.nID
  )

 END AS libId,
 NULL
FROM bts_itemreference bar

UPDATE bts_libreference 
 SET refName = (SELECT nvcName+ '.btl' FROM bts_assembly mod WHERE mod.nID = /*bts_libreference.*/idlib )


RETURN
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_RecalculateAssemblyReferences] TO [BTS_ADMIN_USERS]
    AS [dbo];

