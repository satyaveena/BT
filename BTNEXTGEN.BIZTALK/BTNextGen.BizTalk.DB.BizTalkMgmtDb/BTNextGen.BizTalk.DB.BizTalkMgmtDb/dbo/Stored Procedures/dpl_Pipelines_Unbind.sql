CREATE PROCEDURE [dbo].[dpl_Pipelines_Unbind]
(
 @ModuleId as int
)

AS
set nocount on
set xact_abort on
begin tran

 -- TO DO: IDs of pipelines in module -- same query unnecessarily repeated; factor out using temp table?

 -- unbind send ports receive pipelines from pipelines in module
 update [bts_sendport]
 set [nReceivePipelineID] = null
 where [nReceivePipelineID] in
 (
  -- IDs of pipelines in module
  select [bts_pipeline].[Id]
  from [bts_pipeline] inner join [bts_item] on ([bts_item].[IsPipeline] = 1 and [bts_item].[Guid] = [bts_pipeline].[PipelineID])
  where ([bts_item].[AssemblyId] = @ModuleId)
 )

 -- unbind send ports transmit pipelines from pipelines in module
 update [bts_sendport]
 set [nSendPipelineID] = null
 where [nSendPipelineID] in
 (
  -- IDs of pipelines in module
  select [bts_pipeline].[Id]
  from [bts_pipeline] inner join [bts_item] on ([bts_item].[IsPipeline] = 1 and [bts_item].[Guid] = [bts_pipeline].[PipelineID])
  where ([bts_item].[AssemblyId] = @ModuleId)
 )

 -- unbind receive locations send pipelines from pipelines in module
 update [adm_ReceiveLocation]
 set [ReceivePipelineId] = null
 where [ReceivePipelineId] in
 (
  -- IDs of pipelines in module
  select [bts_pipeline].[Id]
  from [bts_pipeline] inner join [bts_item] on ([bts_item].[IsPipeline] = 1 and [bts_item].[Guid] = [bts_pipeline].[PipelineID])
  where ([bts_item].[AssemblyId] = @ModuleId)
 )

 -- unbind receive ports transmit pipelines from pipelines in module
 update [bts_receiveport]
 set [nSendPipelineId] = null
 where [nSendPipelineId] in
 (
  -- IDs of pipelines in module
  select [bts_pipeline].[Id]
  from [bts_pipeline] inner join [bts_item] on ([bts_item].[IsPipeline] = 1 and [bts_item].[Guid] = [bts_pipeline].[PipelineID])
  where ([bts_item].[AssemblyId] = @ModuleId)
 )

commit tran
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Pipelines_Unbind] TO [BTS_ADMIN_USERS]
    AS [dbo];

