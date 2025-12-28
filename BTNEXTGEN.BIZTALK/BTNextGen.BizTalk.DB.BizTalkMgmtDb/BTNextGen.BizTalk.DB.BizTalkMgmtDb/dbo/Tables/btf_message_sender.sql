CREATE TABLE [dbo].[btf_message_sender] (
    [identity]     VARCHAR (256) NOT NULL,
    [expires_at]   DATETIME      NOT NULL,
    [acknowledged] CHAR (1)      NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [CIX_BTF_Sender]
    ON [dbo].[btf_message_sender]([identity] ASC) WITH (IGNORE_DUP_KEY = ON);

