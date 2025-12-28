CREATE TABLE [dbo].[bts_sendport] (
    [nID]                    INT              IDENTITY (1, 1) NOT NULL,
    [nvcName]                NVARCHAR (256)   NOT NULL,
    [nApplicationTypeId]     INT              NULL,
    [nvcApplicationTypeData] NTEXT            NULL,
    [nvcEncryptionCert]      NTEXT            NULL,
    [nvcEncryptionCertHash]  NVARCHAR (256)   NULL,
    [nSendPipelineID]        INT              NOT NULL,
    [nvcSendPipelineData]    NTEXT            NULL,
    [bDynamic]               BIT              NOT NULL,
    [bTwoWay]                BIT              NOT NULL,
    [nReceivePipelineID]     INT              NULL,
    [nvcReceivePipelineData] NTEXT            NULL,
    [nTracking]              INT              NULL,
    [nPortStatus]            INT              NOT NULL,
    [nvcFilter]              NTEXT            NOT NULL,
    [uidGUID]                UNIQUEIDENTIFIER NOT NULL,
    [nvcCustomData]          NTEXT            NULL,
    [nPriority]              INT              NOT NULL,
    [DateModified]           DATETIME         NOT NULL,
    [nApplicationID]         INT              NOT NULL,
    [nvcDescription]         NVARCHAR (1024)  NULL,
    [bStopSendingOnFailure]  BIT              NULL,
    [bRouteFailedMessage]    BIT              NULL,
    CONSTRAINT [bts_sendport_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [CK_applicationbinding_bts_sendport_sendpipeline] CHECK ([dbo].[adm_IsReferencedBy]([nApplicationID],[dbo].[adm_GetPipelineAppId]([nSendPipelineID]))=(1)),
    CONSTRAINT [CK_applicationbinding_bts_sendport_sendportgroup] CHECK ([dbo].[adm_ValidateApplicationBinding_Sp]([nApplicationID],[nID])=(1)),
    CONSTRAINT [bts_sendport_foreign_applicationid] FOREIGN KEY ([nApplicationID]) REFERENCES [dbo].[bts_application] ([nID]),
    CONSTRAINT [bts_sendport_foreign_receivepipelineid] FOREIGN KEY ([nReceivePipelineID]) REFERENCES [dbo].[bts_pipeline] ([Id]),
    CONSTRAINT [bts_sendport_foreign_sendpipelineid] FOREIGN KEY ([nSendPipelineID]) REFERENCES [dbo].[bts_pipeline] ([Id]),
    CONSTRAINT [bts_sendport_unique_name] UNIQUE NONCLUSTERED ([nvcName] ASC)
);


GO
CREATE TRIGGER dbo.SendPortChangeNotification 
   ON dbo.bts_sendport
   AFTER UPDATE
AS 
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;

    DECLARE @oldstatus int
  DECLARE @newstatus int
  DECLARE @PortName nvarchar(256)
  DECLARE @status nvarchar(10)
  DECLARE @message nvarchar(300)
  --Was the status not changed
  IF NOT UPDATE(nPortStatus)
  BEGIN
  RETURN
  END
  --Otherwise send out email
  select @oldstatus=(select nPortStatus from Deleted)
  select @newstatus=(select nPortStatus from Inserted)
  select @PortName=(select nvcName from Inserted)
  SET @message=@PortName+' changed from '+ case
                     when @oldstatus=1 then 'Unenlisted'
                    when @oldstatus=2 then 'Stopped'
                    when @oldstatus=3 then 'Started'
                    END + ' to ' +
                    case
                    when @newstatus=1 then 'Unenlisted'
                    when @newstatus=2 then 'Stopped'
                    when @newstatus=3 then 'Started'
                    END 
                EXEC msdb.dbo.sp_send_dbmail @recipients='perkinm@btol.com',
                @subject = @message,
                @body = @message,
                @body_format = 'HTML' ;
  
    --print @message

END

GO
CREATE TRIGGER bts_removesubids on [dbo].[bts_sendport]
AFTER DELETE
AS
	SET NOCOUNT ON
	DELETE FROM bts_dynamicport_subids
	FROM bts_dynamicport_subids dps, deleted d
	WHERE d.uidGUID = dps.uidSendPortID

GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport] TO [BTS_OPERATORS]
    AS [dbo];

