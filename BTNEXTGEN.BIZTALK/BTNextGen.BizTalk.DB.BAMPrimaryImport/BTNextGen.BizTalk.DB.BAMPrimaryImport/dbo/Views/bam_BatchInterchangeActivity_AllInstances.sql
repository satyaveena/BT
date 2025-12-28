CREATE VIEW [dbo].[bam_BatchInterchangeActivity_AllInstances]
        AS
          
              SELECT * FROM [dbo].[bam_BatchInterchangeActivity_ActiveInstances]
              UNION ALL 
              SELECT * FROM [dbo].[bam_BatchInterchangeActivity_CompletedInstances]
            
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_BatchInterchangeActivity_AllInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_BatchInterchangeActivity_AllInstances] TO [BTS_OPERATORS]
    AS [dbo];

