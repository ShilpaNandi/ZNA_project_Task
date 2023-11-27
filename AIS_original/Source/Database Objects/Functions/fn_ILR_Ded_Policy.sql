
if exists (select 1 from sysobjects 
		where name = 'fn_ILR_Ded_Policy' and type = 'FN')
	drop function fn_ILR_Ded_Policy
GO
set ansi_nulls off
GO

SET QUOTED_IDENTIFIER ON
GO
  
---------------------------------------------------------------------  
-----  
----- Proc Name:  [fn_ILR_Ded_Policy]   
-----  
----- Version:  SQL Server 2005  
-----  
----- Created:  Suneel Kumar Mogali  
-----  
----- Description: 
-----  
----- Modified:   
-----  
---------------------------------------------------------------------  
CREATE function [dbo].[fn_ILR_Ded_Policy]  
 (  
 @ADJNO int,   
 @PERDID int    
 )  
returns int  
--WITH SCHEMABINDING  
as  
begin  
  
 declare @adj_typ_id int  
  
 set @adj_typ_id = 0  
  
IF EXISTS (select top 1 *  
from PREM_ADJ_PARMET_DTL  
inner join PREM_ADJ_PARMET_SETUP on PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id=PREM_ADJ_PARMET_DTL.prem_adj_parmet_setup_id  
inner join COML_AGMT on COML_AGMT.coml_agmt_id=PREM_ADJ_PARMET_DTL.coml_agmt_id  
where PREM_ADJ_PARMET_SETUP.prem_adj_perd_id = @PERDID   
and PREM_ADJ_PARMET_SETUP.prem_adj_id = @ADJNO  
and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id = 398  
and COML_AGMT.adj_typ_id NOT IN(62,68,448))  
  
BEGIN  
set @adj_typ_id = 0 
END  
ELSE  
BEGIN  
set @adj_typ_id = 1  
END  
  
  
return @adj_typ_id  
  
end  
   
GO
if object_id('fn_ILR_Ded_Policy') is not null
	print 'Created function fn_ILR_Ded_Policy'
else
	print 'Failed function fn_ILR_Ded_Policy'
go

if object_id('fn_ILR_Ded_Policy') is not null
	grant exec on fn_ILR_Ded_Policy to public
go 
 

