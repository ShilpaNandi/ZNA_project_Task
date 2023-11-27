if exists (select 1 from sysobjects 
		where name = 'fn_GetPolCheck' and type = 'FN')
	drop function fn_GetPolCheck
GO
set ansi_nulls on
GO


SET QUOTED_IDENTIFIER ON
GO

  
---------------------------------------------------------------------  
-----  
----- Proc Name:  fn_GetPolCheck  
-----  
----- Version:  SQL Server 2005  
-----  
----- Created:  Suneel Kumar Mogali  
-----  
----- Description: Tells whether coml_agmt_id present in prem_adj_pgm_setup_pol table.  
-----  
----- Modified:   07/16/2015 : Added adj_parmet_typ_id 399 for fixing the issue with INC0862333(The Loss Fund exhibit is missing the policy Numbers)
-----  
---------------------------------------------------------------------  
CREATE FUNCTION [dbo].[fn_GetPolCheck]  
(@comlagmtid int,  
@adjid int,  
@perdid int,  
@typnum int)  
RETURNS int   
AS  
BEGIN  
  
declare @Return int  
set @Return = 0  
  
IF EXISTS(
select 1 from prem_adj_pgm_setup_pol Inner join  
prem_adj_pgm_setup on prem_adj_pgm_setup.prem_adj_pgm_setup_id =  
prem_adj_pgm_setup_pol.prem_adj_pgm_setup_id and  
prem_adj_pgm_setup.adj_parmet_typ_id IN (398,399,400)  
inner join prem_adj_pgm on prem_adj_pgm.prem_adj_pgm_id = prem_adj_pgm_setup_pol.  
prem_adj_pgm_id and prem_adj_pgm.custmr_id = prem_adj_pgm_setup_pol.custmr_id  
inner join prem_adj_perd on prem_adj_perd.prem_adj_pgm_id = prem_adj_pgm.prem_adj_pgm_id  
and prem_adj_perd.prem_adj_id = @adjid and prem_adj_perd.prem_adj_perd_id = @perdid  
where coml_agmt_id = @comlagmtid 
) 
BEGIN
	SET @Return = 1
END
  
  
return @Return  
end  
  

  
GO
if object_id('fn_GetPolCheck') is not null
	print 'Created function fn_GetPolCheck'
else
	print 'Failed function fn_GetPolCheck'
go

if object_id('fn_GetPolCheck') is not null
	grant exec on fn_GetPolCheck to public
go 

