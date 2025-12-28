CREATE VIEW [dbo].[bam_ResendTrackingActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_ResendTrackingActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_ResendTrackingActivity_CompletedInstances]
            