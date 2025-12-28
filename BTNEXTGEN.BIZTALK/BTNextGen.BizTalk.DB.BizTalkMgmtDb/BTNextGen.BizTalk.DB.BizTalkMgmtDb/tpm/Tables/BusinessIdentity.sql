CREATE TABLE [tpm].[BusinessIdentity] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ProfileId]      INT            NULL,
    [Name]           NVARCHAR (256) NOT NULL,
    [Description]    NVARCHAR (256) NULL,
    [Qualifier]      NVARCHAR (64)  NOT NULL,
    [Value]          NVARCHAR (256) NOT NULL,
    [AdditionalData] NVARCHAR (256) NULL,
    [DateModified]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [version]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_BusinessIdentity] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_BusinessIdentity_Name] CHECK ([Qualifier]<>'OrganizationName'),
    CONSTRAINT [FK_BusinessIdentity_BusinessProfile] FOREIGN KEY ([ProfileId]) REFERENCES [tpm].[BusinessProfile] ([ProfileId]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_Qualifier_Value_NonNullProfile]
    ON [tpm].[BusinessIdentity]([ProfileId] ASC, [Qualifier] ASC, [Value] ASC) WHERE ([ProfileId] IS NOT NULL);


GO
CREATE TRIGGER [tpm].[BusinessIdentityTrigger] ON [tpm].[BusinessIdentity]
FOR INSERT, UPDATE AS
    DECLARE c CURSOR FOR 
    SELECT [Qualifier],[Value] FROM inserted
    DECLARE @qualifier [nvarchar](64)
    DECLARE @value [nvarchar](256)
    DECLARE @partnerCount [int]
    
    OPEN c 
    FETCH NEXT FROM c INTO @qualifier, @value
    WHILE @@FETCH_STATUS = 0 
    BEGIN 
        SELECT @partnerCount = count (DISTINCT PartnerId)
        FROM tpm.BusinessProfile AS BP 
        inner join tpm.BusinessIdentity AS BI 
        on BP.ProfileId = BI.ProfileId
        WHERE BI.Qualifier = @qualifier AND BI.Value = @value
        
        IF @partnerCount > 1 
        BEGIN 
            ROLLBACK TRANSACTION
            RAISERROR(90000, 16, 1, @qualifier, @value)
            BREAK
        END
        FETCH NEXT FROM c INTO @qualifier, @value
    END 
    CLOSE c 
    DEALLOCATE c 
    RETURN
