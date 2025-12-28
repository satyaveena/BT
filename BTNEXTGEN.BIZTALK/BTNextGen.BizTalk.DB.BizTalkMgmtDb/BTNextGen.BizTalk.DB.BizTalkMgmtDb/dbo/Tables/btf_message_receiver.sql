CREATE TABLE [dbo].[btf_message_receiver] (
    [identity]   VARCHAR (256) NOT NULL,
    [expires_at] DATETIME      NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [CIX_BTF_Receiver]
    ON [dbo].[btf_message_receiver]([identity] ASC) WITH (IGNORE_DUP_KEY = ON);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BizTalk framework pipeline component message tracking table for receiver', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'btf_message_receiver';

