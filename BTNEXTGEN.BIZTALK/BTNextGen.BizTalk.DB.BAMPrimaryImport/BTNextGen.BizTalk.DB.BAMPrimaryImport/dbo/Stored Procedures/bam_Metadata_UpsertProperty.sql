CREATE PROCEDURE [dbo].[bam_Metadata_UpsertProperty]
(
    @propertyName            sysname,
    @propertyValue            ntext,
    @scope                  sysname = N'Global'
)
AS
    BEGIN TRAN
        SELECT PropertyName FROM dbo.bam_Metadata_Properties
        WHERE (Scope = @scope AND PropertyName = @propertyName)
        
        IF @@ROWCOUNT = 0    
            INSERT dbo.bam_Metadata_Properties (Scope, PropertyName, PropertyValue)
            VALUES (@scope, @propertyName, @propertyValue)    
        ELSE
            UPDATE dbo.bam_Metadata_Properties
            SET PropertyValue = @propertyValue
            WHERE (Scope = @scope AND PropertyName = @propertyName)
    COMMIT TRAN