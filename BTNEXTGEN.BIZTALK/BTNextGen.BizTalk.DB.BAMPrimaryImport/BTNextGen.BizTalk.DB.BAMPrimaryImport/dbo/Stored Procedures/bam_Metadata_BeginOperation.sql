-- Insert into Operations table
CREATE PROCEDURE [dbo].[bam_Metadata_BeginOperation]
(
    @userName                    sysname,
    @operationType                NVARCHAR(10),
    @originalDefinitionXml        NTEXT,
    @workingDefinitionXml        NTEXT,
    @bamConfigurationXml        NTEXT,
    @bamDefinitionFileName        NVARCHAR(1024),
    @assemblyVersion            sysname
)
AS
    INSERT dbo.[bam_Metadata_Operations]
        (UserLogin, OperationType, StartTime, OriginalDefinitionXml, WorkingDefinitionXml, BamConfigurationXml, 
         BamDefinitionFileName, BamManagerFileVersion)
    VALUES
        (@userName, @operationType, GETUTCDATE(), @originalDefinitionXml, @workingDefinitionXml, @bamConfigurationXml, 
        @bamDefinitionFileName, @assemblyVersion)
    
    -- Return new operation record ID
    SELECT CAST(@@IDENTITY AS INT)