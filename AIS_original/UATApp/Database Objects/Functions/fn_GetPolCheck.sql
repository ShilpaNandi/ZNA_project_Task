
if exists (select 1 from sysobjects 
		where name = 'fn_GetPolCheck' and type = 'FN')
	drop function fn_GetPolCheck
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPolCheck
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Tells whether coml_agmt_id present in prem_adj_pgm_setup_pol table.
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE FUNCTION fn_GetPolCheck
(@comlagmtid int,
@adjid int,
@perdid int,
@typnum int)
RETURNS int 
AS
BEGIN

declare @Return int
set @Return = 0

select @Return = 1 from prem_adj_pgm_setup_pol Inner join
prem_adj_pgm_setup on prem_adj_pgm_setup.prem_adj_pgm_setup_id =
prem_adj_pgm_setup_pol.prem_adj_pgm_setup_id and
prem_adj_pgm_setup.adj_parmet_typ_id = @typnum
inner join prem_adj_pgm on prem_adj_pgm.prem_adj_pgm_id = prem_adj_pgm_setup_pol.
prem_adj_pgm_id and prem_adj_pgm.custmr_id = prem_adj_pgm_setup_pol.custmr_id
inner join prem_adj_perd on prem_adj_perd.prem_adj_pgm_id = prem_adj_pgm.prem_adj_pgm_id
and prem_adj_perd.prem_adj_id = @adjid and prem_adj_perd.prem_adj_perd_id = @perdid
where coml_agmt_id = @comlagmtid


return @Return
end

go

if object_id('fn_GetPolCheck') is not null
	print 'Created function fn_GetPolCheck'
else
	print 'Failed function fn_GetPolCheck'
go

if object_id('fn_GetPolCheck') is not null
	grant exec on fn_GetPolCheck to public
go
