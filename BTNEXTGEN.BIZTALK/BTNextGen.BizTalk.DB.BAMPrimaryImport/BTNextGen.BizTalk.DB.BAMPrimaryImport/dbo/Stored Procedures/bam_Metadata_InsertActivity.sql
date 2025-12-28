CREATE PROCEDURE [dbo].[bam_Metadata_InsertActivity]
(
    @activityName NVARCHAR(128),
    @definitionXml NTEXT
)
AS
-- Register activity in the metadata table
INSERT [dbo].[bam_Metadata_Activities]
(
    ActivityName,
    OnlineWindowTimeUnit,
    OnlineWindowTimeLength,
    DefinitionXml
)
VALUES
(
    @activityName,
    'MONTH', 
    6,
    @definitionXml
)