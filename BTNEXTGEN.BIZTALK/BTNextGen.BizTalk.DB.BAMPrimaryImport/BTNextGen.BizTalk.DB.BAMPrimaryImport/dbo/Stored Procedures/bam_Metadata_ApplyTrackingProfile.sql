CREATE PROCEDURE [dbo].[bam_Metadata_ApplyTrackingProfile]
(
    @ActivityName nvarchar(128),
    @ReferencedBy nvarchar(300),
    @VersionId uniqueidentifier,
    @ProfileXml ntext
)
AS
    DECLARE @MinorVersionId int
    SET @MinorVersionId = 0

    SELECT    @MinorVersionId = MinorVersionId
    FROM    [dbo].[bam_Metadata_TrackingProfiles]
    WHERE    ActivityName = @ActivityName
        AND    ReferencedBy = @ReferencedBy

    SET @MinorVersionId = @MinorVersionId + 1

    -- If the MinorVersion is 1, that means that this profile did not
    -- exist before, so insert it into the table
    -- If it is greater than 1, then the profile existed before, so
    -- update it.
    -- It cannot be less than 0.
    IF (@MinorVersionId = 1)
    BEGIN
        INSERT INTO [dbo].[bam_Metadata_TrackingProfiles]
        (
            ActivityName,
            ReferencedBy,
            VersionId, 
            MinorVersionId,
            ProfileXml
        ) 
        VALUES
        (
            @ActivityName,
            @ReferencedBy,
            @VersionId, 
            @MinorVersionId,
            @ProfileXml
        )
    END
    ELSE
    BEGIN    
        UPDATE     [dbo].[bam_Metadata_TrackingProfiles]
        SET        ProfileXml = @ProfileXml,
                MinorVersionId = @MinorVersionId
        WHERE    ReferencedBy = @ReferencedBy
            AND    ActivityName = @ActivityName
    END
    
    SELECT @MinorVersionId