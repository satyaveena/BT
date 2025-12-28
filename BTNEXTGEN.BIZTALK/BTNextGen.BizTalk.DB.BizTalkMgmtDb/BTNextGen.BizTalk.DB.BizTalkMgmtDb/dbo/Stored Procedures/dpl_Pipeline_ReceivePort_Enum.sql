CREATE PROCEDURE [dbo].[dpl_Pipeline_ReceivePort_Enum]
(
 @PipeId as int
)

AS
set nocount on
set xact_abort on

select * from [bts_receiveport]
where ([nSendPipelineId] = @PipeId)
order by [nvcName]

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Pipeline_ReceivePort_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Pipeline_ReceivePort_Enum] TO [BTS_OPERATORS]
    AS [dbo];

