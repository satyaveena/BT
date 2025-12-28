CREATE PROCEDURE [dbo].[bts_removescalarfunctions]
AS
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_removescalarfunctions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[bts_removescalarfunctions]
	
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_OrchestrationBindingComplete]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1) drop function [dbo].[bts_OrchestrationBindingComplete]
	
