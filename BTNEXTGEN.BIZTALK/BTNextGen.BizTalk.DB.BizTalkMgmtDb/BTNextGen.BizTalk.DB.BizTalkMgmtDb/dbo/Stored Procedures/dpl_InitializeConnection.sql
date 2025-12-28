CREATE PROCEDURE [dbo].[dpl_InitializeConnection]

AS
set nocount on

SET DEADLOCK_PRIORITY LOW -- Prevent disrupting runtime (deployment can be always re-ran later, without data loss) 
        -- by agreeing to be selected as default deadlock victim

-- DECLARE @asmID int
-- SELECT @asmID = (SELECT TOP 1 nID
-- FROM bts_assembly WITH (HOLDLOCK,TABLOCKX) -- Implement deployment DB semaphore (lessen the probability of deadlocks, this is first DB operation of Deployment)
-- WHERE nSystemAssembly <> 0)

-- SET TEXTSIZE 2000000000 -- enable support for large blobs (this is probably set by ADO.NET by default, but just to be sure)

RETURN
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_InitializeConnection] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_InitializeConnection] TO [BTS_OPERATORS]
    AS [dbo];

