CREATE TABLE [dbo].[bam_PaymentERPTxn_Continuations] (
    [ActivityID]       NVARCHAR (128) NOT NULL,
    [ParentActivityID] NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [NCI_ParentActivityID]
    ON [dbo].[bam_PaymentERPTxn_Continuations]([ParentActivityID] ASC);

