CREATE VIEW [dbo].[bam_ResendTrackingActivity_InstancesForArchive]
        AS
          
              SELECT TOP 0 * FROM [dbo].[bam_ResendTrackingActivity_Completed] WITH (NOLOCK)
            