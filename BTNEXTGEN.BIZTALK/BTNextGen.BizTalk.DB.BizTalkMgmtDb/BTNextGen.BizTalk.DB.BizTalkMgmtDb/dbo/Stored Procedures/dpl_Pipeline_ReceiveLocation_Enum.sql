CREATE PROCEDURE [dbo].[dpl_Pipeline_ReceiveLocation_Enum]
(
 @PipeId as int
)

AS
set nocount on
set xact_abort on

select * from [adm_ReceiveLocation]
where ([ReceivePipelineId] = @PipeId)
order by [Name]

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Pipeline_ReceiveLocation_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Pipeline_ReceiveLocation_Enum] TO [BTS_OPERATORS]
    AS [dbo];

