CREATE TABLE [dbo].[bam_AS2MdnActivity_Completed] (
    [RecordID]                          BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActivityID]                        NVARCHAR (128)  NOT NULL,
    [AS2PartyRole]                      INT             NULL,
    [AS2From]                           NVARCHAR (128)  NULL,
    [AS2To]                             NVARCHAR (128)  NULL,
    [MessageID]                         NVARCHAR (1000) NULL,
    [MdnDateTime]                       DATETIME        NULL,
    [MdnDispositionType]                INT             NULL,
    [DispositionModifierExtType]        INT             NULL,
    [DispositionModifierExtDescription] INT             NULL,
    [MdnEncryptionType]                 INT             NULL,
    [MdnSignatureType]                  INT             NULL,
    [MdnPayloadContentKey]              NVARCHAR (40)   NULL,
    [MdnWireContentKey]                 NVARCHAR (40)   NULL,
    [MdnMicValue]                       NVARCHAR (50)   NULL,
    [TimeCreated]                       DATETIME        NULL,
    [RowFlags]                          INT             NULL,
    [AgreementName]                     NVARCHAR (256)  NULL,
    [LastModified]                      DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_AS2MdnActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_AS2MdnActivity_Completed]([LastModified] ASC);

