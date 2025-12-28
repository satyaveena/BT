CREATE PROCEDURE [dbo].[btf_PurgeExpiredMessages]
AS
    SET NOCOUNT ON
    DECLARE @currTime AS DATETIME
    SET @currTime = GetUtcDate();
    
    EXEC btf_sender_expired_delete @currTime 
    EXEC btf_receiver_expired_delete @currTime
    
