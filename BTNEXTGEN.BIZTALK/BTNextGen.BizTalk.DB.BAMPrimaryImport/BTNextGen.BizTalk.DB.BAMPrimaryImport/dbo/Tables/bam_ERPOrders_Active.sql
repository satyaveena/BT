CREATE TABLE [dbo].[bam_ERPOrders_Active] (
    [ActivityID]      NVARCHAR (128) NOT NULL,
    [IsVisible]       BIT            DEFAULT (NULL) NULL,
    [IsComplete]      BIT            DEFAULT (NULL) NULL,
    [PONum]           NVARCHAR (50)  NULL,
    [TransNum]        NVARCHAR (50)  NULL,
    [AccountNum]      NVARCHAR (50)  NULL,
    [TargetERP]       NVARCHAR (20)  NULL,
    [PORcvd]          DATETIME       NULL,
    [POSentERP]       DATETIME       NULL,
    [ERPAckRcv]       DATETIME       NULL,
    [NGRHeaderUpdate] DATETIME       NULL,
    [NGRLineUpdate]   DATETIME       NULL,
    [CSAckUpdate]     DATETIME       NULL,
    [LastModified]    DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

