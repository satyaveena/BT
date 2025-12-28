CREATE PROCEDURE [dbo].[procGetLatestOrderAgeBAM]

AS
	
	-- Name: procGetLatestOrderAgeBAM
	-- Author: Marvin Perkins
	-- Description: Finds the most recent order that ERPOrders created events for in BAM and returns the particulars as well as an age in minutes.
	--		This will serve as the basis of an alert when it is fair to determine something is wrong if new orders don't come in every x minutes.

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- the timestamp columns in BAM is stored in UTC. So DateADD function is used to convert the utc to local
	
	SET NOCOUNT ON;
	
	
		SELECT TOP(1) A.ActivityID ,A.PONum,A.TransNum,A.AccountNum,A.TargetERP,DATEADD(minute, DATEDIFF(minute,getutcdate(),getdate()),A.PORcvd) AS PORcvd
		,DATEADD(minute, DATEDIFF(minute,getutcdate(),getdate()),A.POSentERP) AS POSentERP,DATEADD(minute, DATEDIFF(minute,getutcdate(),getdate()),A.LastModified)
		,(DATEDIFF(MINUTE,DATEADD(minute, DATEDIFF(minute,getutcdate(),getdate()),A.PORcvd),GETDATE())) as AgeInMinutes
		FROM BAMPrimaryImport.[dbo].bam_ERPOrders_AllInstances A(NOLOCK)
		ORDER BY A.PORcvd DESC	
	RETURN 0
END
