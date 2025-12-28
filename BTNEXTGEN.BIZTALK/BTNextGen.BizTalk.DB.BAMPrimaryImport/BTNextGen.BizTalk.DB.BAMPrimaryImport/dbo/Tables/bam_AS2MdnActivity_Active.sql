CREATE TABLE [dbo].[bam_AS2MdnActivity_Active] (
    [ActivityID]                        NVARCHAR (128)  NOT NULL,
    [IsVisible]                         BIT             DEFAULT (NULL) NULL,
    [IsComplete]                        BIT             DEFAULT (NULL) NULL,
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
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

