CREATE FUNCTION [dbo].[adm_GetOrchestrationDependencies]
(
 @nOrchestrationID int,
 @Direction nchar(1)  -- 'U' upstream / 'D' downstream
)
RETURNS @tblServiceDependency TABLE (CallerSvcId int,
          CalleeSvcId int,
          CallType tinyint,
          Depth int)
AS
BEGIN
 declare @nCurrServiceId as int, @nStackSize as int
 
 -- Declare a Stack
 declare @tblStack TABLE(
  id integer identity(1,1),
  value integer NOT NULL,
  Depth int NOT NULL
 )

 -- Push root into Stack
 insert into @tblStack values(@nOrchestrationID, 0)
 set @nStackSize = 1

 while ( @nStackSize > 0 )
 begin
  -- Pop top entry from Stack
  declare @id as int, @nCurrDepth as int
  select @id = id, @nCurrServiceId = value, @nCurrDepth = Depth from @tblStack order by id asc
  delete from @tblStack where id = @id

  -- Declare Table for storing new dependencies found
  declare @tblNewDependency TABLE(
   CallerSvcId int NOT NULL,
   CalleeSvcId int NOT NULL,
   CallType tinyint NOT NULL,
   Depth int NOT NULL
  )
  
  -- Find the next layer of new dependencies
  insert into @tblNewDependency
   select nOrchestrationID, nInvokedOrchestrationID, nInvokeType, @nCurrDepth+1
   from bts_orchestration_invocation
   where (@Direction = N'D' AND nOrchestrationID = @nCurrServiceId)
     OR
      (@Direction = N'U' AND nInvokedOrchestrationID = @nCurrServiceId)
   
  if ( @@ROWCOUNT > 0 )
  begin
   -- Push only those new depending ServiceIds into Stack only if they have
   -- not been visited before
   if ( @Direction = N'D' )
    begin
     insert into @tblStack
      select CalleeSvcId, @nCurrDepth+1
      from @tblNewDependency
      where CalleeSvcId <> @nOrchestrationID AND
       CalleeSvcId NOT IN ( select CalleeSvcId
            from @tblServiceDependency )
    end           
   else
    begin
     insert into @tblStack
      select CallerSvcId, @nCurrDepth+1
      from @tblNewDependency
      where CallerSvcId <> @nOrchestrationID AND
       CallerSvcId NOT IN ( select CallerSvcId
            from @tblServiceDependency )
    end           

   -- Aggregate all new dependencies found into @tblServiceDependency
   insert into @tblServiceDependency
    select CallerSvcId, CalleeSvcId, CallType, Depth
    from @tblNewDependency
  end

  -- Reset @tblNewDependency
  delete from @tblNewDependency

  -- Get new stack size  
  select @nStackSize = count(*) from @tblStack
 end

 return
END