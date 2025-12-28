CREATE TABLE [dbo].[bam_AS2MessageActivity_Continuations] (
    [ActivityID]       NVARCHAR (128) NOT NULL,
    [ParentActivityID] NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [NCI_ParentActivityID]
    ON [dbo].[bam_AS2MessageActivity_Continuations]([ParentActivityID] ASC);

