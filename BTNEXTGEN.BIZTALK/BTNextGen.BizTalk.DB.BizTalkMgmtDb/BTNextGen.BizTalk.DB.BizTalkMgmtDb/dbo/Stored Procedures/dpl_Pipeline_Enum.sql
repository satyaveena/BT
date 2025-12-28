CREATE PROCEDURE [dbo].[dpl_Pipeline_Enum]
(
 @ModuleId as int
)

AS
set nocount on
set xact_abort on

select
 [bts_pipeline].[Id] as [Id],
 [bts_pipeline].[PipelineID] as [PipelineID],
 [bts_pipeline].[Category] as [Category],
 [bts_pipeline].[Name] as [Name],
 [bts_pipeline].[FullyQualifiedName] as [FullyQualifiedName]
from [bts_item] inner join [bts_pipeline] on ([Guid] = [PipelineID])
where
 ([IsPipeline] = 1) and
 ([AssemblyId] = @ModuleId)
order by [bts_pipeline].[FullyQualifiedName]

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Pipeline_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Pipeline_Enum] TO [BTS_OPERATORS]
    AS [dbo];

