CREATE VIEW [dbo].[bam_BatchingActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_BatchingActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_BatchingActivity_CompletedInstances]
            
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_BatchingActivity_AllInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_BatchingActivity_AllInstances] TO [BTS_OPERATORS]
    AS [dbo];

