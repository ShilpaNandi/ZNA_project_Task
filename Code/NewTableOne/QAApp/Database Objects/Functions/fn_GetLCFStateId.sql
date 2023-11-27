if exists (select 1 from sysobjects 
		where name = 'fn_GetLCFStateId' and type = 'FN')
	drop function fn_GetLCFStateId
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetLCFStateId
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves State Id's for LCF
-----
-----	Modified:	23-Jan-2009-Suneel- Added brackets to list of state id's returned by the function.
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetLCFStateId]
   (
	@PGMID int	
	)
returns VARCHAR(8000)

as
begin
	declare @sstring varchar(8000)
	set @sstring = null
	select @sstring =Coalesce(@sstring + ',', '') + CONVERT(VARCHAR(10),PREM_ADJ_PGM_DTL.ST_ID) 
	from PREM_ADJ_PGM_DTL 
	INNER JOIN PREM_ADJ_PGM_SETUP ON PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID = 
	PREM_ADJ_PGM_DTL.PREM_ADJ_PGM_SETUP_ID	
	WHERE PREM_ADJ_PGM_DTL.PREM_ADJ_PGM_ID =  @PGMID 
	AND PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 402
	AND PREM_ADJ_PGM_DTL.ST_ID <> 3
	if @sstring is not null
	set @sstring=' ('+@sstring+')'
	return @sstring
end

go

if object_id('fn_GetLCFStateId') is not null
	print 'Created function fn_GetLCFStateId'
else
	print 'Failed Creating Function fn_GetLCFStateId'
go

if object_id('fn_GetLCFStateId') is not null
	grant exec on fn_GetLCFStateId to public
go
