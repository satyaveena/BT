CREATE TABLE [dbo].[adm_ReceiveLocation] (
    [Id]                       INT              IDENTITY (1, 1) NOT NULL,
    [AdapterId]                INT              NOT NULL,
    [Name]                     NVARCHAR (256)   NOT NULL,
    [ReceiveHandlerId]         INT              NULL,
    [GroupId]                  INT              NULL,
    [DateModified]             DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [Comment]                  NVARCHAR (256)   NOT NULL,
    [OperatingWindowEnabled]   INT              NOT NULL,
    [ActiveStartDT]            DATETIME         NULL,
    [ActiveStopDT]             DATETIME         NULL,
    [StartDTEnabled]           BIT              NOT NULL,
    [SrvWinStartDT]            DATETIME         NULL,
    [StopDTEnabled]            BIT              NOT NULL,
    [SrvWinStopDT]             DATETIME         NULL,
    [Disabled]                 INT              NOT NULL,
    [CustomCfg]                NTEXT            NULL,
    [uidCustomCfgID]           UNIQUEIDENTIFIER NULL,
    [bSSOMappingExists]        BIT              NOT NULL,
    [InboundTransportURL]      NVARCHAR (256)   NOT NULL,
    [InboundAddressableURL]    NVARCHAR (256)   NULL,
    [ReceivePipelineId]        INT              NULL,
    [ReceivePipelineData]      NTEXT            NULL,
    [ReceivePortId]            INT              NOT NULL,
    [IsPrimary]                INT              NOT NULL,
    [Fragmentation]            INT              DEFAULT ((0)) NOT NULL,
    [nvcCustomData]            NTEXT            NULL,
    [SendPipelineId]           INT              NULL,
    [EncryptionCert]           NVARCHAR (256)   NULL,
    [EncryptionCertThumbPrint] NVARCHAR (80)    NULL,
    [Description]              NTEXT            NULL,
    [SendPipelineData]         NTEXT            NULL,
    CONSTRAINT [adm_ReceiveLocation_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_ReceiveLocation_EnabledOnlyWhenHasHost] CHECK ([Disabled]<>(0) OR [ReceiveHandlerId] IS NOT NULL),
    CONSTRAINT [CK_applicationbinding_adm_ReceiveLocation_receivepipeline] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetReceivePortAppId]([ReceivePortId]),[dbo].[adm_GetPipelineAppId]([ReceivePipelineId]))=(1)),
    CONSTRAINT [CK_applicationbinding_adm_ReceiveLocation_sendpipeline] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetReceivePortAppId]([ReceivePortId]),[dbo].[adm_GetPipelineAppId]([SendPipelineId]))=(1)),
    CONSTRAINT [adm_ReceiveLocation_fk_adapter] FOREIGN KEY ([AdapterId]) REFERENCES [dbo].[adm_Adapter] ([Id]),
    CONSTRAINT [adm_ReceiveLocation_fk_group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[adm_Group] ([Id]),
    CONSTRAINT [adm_ReceiveLocation_fk_Pipeline] FOREIGN KEY ([ReceivePipelineId]) REFERENCES [dbo].[bts_pipeline] ([Id]),
    CONSTRAINT [adm_ReceiveLocation_fk_ReceivePort] FOREIGN KEY ([ReceivePortId]) REFERENCES [dbo].[bts_receiveport] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [adm_ReceiveLocation_fk_RH] FOREIGN KEY ([ReceiveHandlerId]) REFERENCES [dbo].[adm_ReceiveHandler] ([Id]),
    CONSTRAINT [adm_ReceiveLocation_fk_SendPipeline] FOREIGN KEY ([SendPipelineId]) REFERENCES [dbo].[bts_pipeline] ([Id]),
    CONSTRAINT [adm_ReceiveLocation_unique_key] UNIQUE NONCLUSTERED ([InboundTransportURL] ASC),
    CONSTRAINT [adm_ReceiveLocation_unique_key1] UNIQUE NONCLUSTERED ([Name] ASC, [GroupId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [adm_ReceiveLocation_ix_RH]
    ON [dbo].[adm_ReceiveLocation]([ReceiveHandlerId] ASC);


GO
CREATE TRIGGER [dbo].[ReceiveLocationChangeNotification] 
   ON [dbo].[adm_ReceiveLocation]
   AFTER UPDATE
AS 
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;

    DECLARE @oldstatus int
  DECLARE @newstatus int
  DECLARE @ReceiveLocationName nvarchar(256)
  DECLARE @status nvarchar(10)
  DECLARE @message nvarchar(300)
  --Was the status not changed
  IF NOT UPDATE([Disabled])
  BEGIN
  RETURN
  END
  --Otherwise send out email
  select @oldstatus=(select [Disabled] from Deleted)
  select @newstatus=(select [Disabled] from Inserted)
  select @ReceiveLocationName=(select [Name] from Inserted)
  SET @message=@ReceiveLocationName+' recieve location changed from '+ case
                     when @oldstatus=-1 then 'Disabled'
                    when @oldstatus=0 then 'Enabled'
                    END + ' to ' +
                    case
                    when @newstatus=-1 then 'Disabled'
                    when @newstatus=0 then 'Enabled'
                    END 
                EXEC msdb.dbo.sp_send_dbmail @recipients='perkinm@btol.com',
                @subject = @message,
                @body = @message,
                @body_format = 'HTML' ;

--  print @message

END

GO
GRANT DELETE
    ON OBJECT::[dbo].[adm_ReceiveLocation] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[adm_ReceiveLocation] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_ReceiveLocation] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[adm_ReceiveLocation] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_ReceiveLocation] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_ReceiveLocation] TO [BTS_OPERATORS]
    AS [dbo];

