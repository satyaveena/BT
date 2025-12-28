



-- =============================================
-- Author:		Mahesh Chandak/Marvin Perkins
-- Create date: 09/29/2011
-- Description:	Gets all records from ERP Orders Bam view which didnt get an ACK in last x minutes
-- =============================================

CREATE PROCEDURE [dbo].[procGetNoAckERPOrdersBAM]
	 @DURATION INT
AS
BEGIN

	-- Notes: ===============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- the timestamp columns in BAM is stored in UTC. So DateADD function is used to convert the utc to local
	
	-- 11/28/2011 modified the datediff function to compare using minutes instead of hours.

	-- 02-18-2013 Incorporated into VS.2010 Data Tools Project (from add on).
	--	Author going forward:	Marvin Perkins
	--	Removed div by 60 in prep for tightening "non-acked" window to be minutes based.
	--	"Published" Locally using VS2010 to deploy the changes.

	-- End Notes: =============================================

	SET NOCOUNT ON;
	
	
		SELECT A.ActivityID ,A.PONum,A.TransNum,A.AccountNum,A.TargetERP,DATEADD(minute, DATEDIFF(minute,getutcdate(),getdate()),A.PORcvd) AS PORcvd
		,DATEADD(minute, DATEDIFF(minute,getutcdate(),getdate()),A.POSentERP) AS POSentERP,DATEADD(minute, DATEDIFF(minute,getutcdate(),getdate()),A.LastModified)
		FROM BAMPrimaryImport.[dbo].bam_ERPOrders_AllInstances A(NOLOCK) 
		WHERE A.POSentERP IS NOT NULL AND A.ERPAckRcv IS NULL AND (DATEDIFF(MINUTE,DATEADD(minute, DATEDIFF(minute,getutcdate(),getdate()),A.LastModified),GETDATE())) >= @DURATION
		
		AND A.ActivityID 
		NOT IN (SELECT ActivityID FROM BAMPrimaryImport.[dbo].BT_BAMAlertHistory WHERE AlertExpired =1)
		order by A.PORcvd DESC
		
		
	
END