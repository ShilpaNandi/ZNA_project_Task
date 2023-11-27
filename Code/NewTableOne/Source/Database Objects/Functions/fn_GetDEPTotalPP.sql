if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPTotalPP' and type = 'FN')
	drop function fn_GetDEPTotalPP
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetDEPTotalPP
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Returns totals per PP for DEP
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetDEPTotalPP]
  (
	@ADJID int,
	@PGMID int,
	@CUSTID int,
	@STID int    
	)
returns int
--WITH SCHEMABINDING
as
begin
	declare @result int	
	set @result = 0	
	
	select @result = sum(PREM_ADJ_PARMET_DTL.los_base_asses_amt/
	dbo.fn_GetDEPNumberPolicy(PREM_ADJ_PARMET_DTL.prem_adj_id,PREM_ADJ_PARMET_DTL.prem_adj_pgm_id,
	PREM_ADJ_PARMET_DTL.custmr_id,PREM_ADJ_PARMET_DTL.st_id)) from PREM_ADJ_PARMET_DTL
	where prem_adj_id = @ADJID and prem_adj_pgm_id = @PGMID and custmr_id = @CUSTID

	
	return @result
end

go

if object_id('fn_GetDEPTotalPP') is not null
	print 'Created function fn_GetDEPTotalPP'
else
	print 'Failed Creating Function fn_GetDEPTotalPP'
go

if object_id('fn_GetDEPTotalPP') is not null
	grant exec on fn_GetDEPTotalPP to public
go
