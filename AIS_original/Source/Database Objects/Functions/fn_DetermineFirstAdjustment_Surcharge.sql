if exists (select 1 from sysobjects 
		where name = 'fn_DetermineFirstAdjustment_Surcharge' and type = 'FN')
	drop function fn_DetermineFirstAdjustment_Surcharge
GO
set ansi_nulls off
GO
set quoted_identifier on
GO


---------------------------------------------------------------------
-----
-----	Proc Name:		fn_DetermineFirstAdjustment_Surcharge
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC(Venkat Kolimi) 
-----
-----	Description:	This function  to Determine first adjustment for this surcharge on this policy under the program period.
-----
-----					
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
CREATE function [dbo].[fn_DetermineFirstAdjustment_Surcharge]
   (
		@premium_adj_prog_id int,
		@coml_agmt_id int,
		@st_id int,
		@ln_of_bsn_id int,
		@surchrg_cd_id int,
		@surchrg_typ_id int

	)
returns smallint
as
begin
	declare @cnt_prev_adjs smallint

	/**************************
	* Determine first adjustment
	**************************/

	select @cnt_prev_adjs =count(distinct pa.prem_adj_id) from dbo.PREM_ADJ pa
	inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.reg_custmr_id = ps.custmr_id)
	inner join dbo.PREM_ADJ_PERD prd on (pa.reg_custmr_id = prd.reg_custmr_id) and (pa.prem_adj_id = prd.prem_adj_id)
	inner join dbo.PREM_ADJ_SURCHRG_DTL sd on (pa.prem_adj_id = sd.prem_adj_id )
	where ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
	and prd.prem_adj_pgm_id = @premium_adj_prog_id
    and pa.adj_can_ind<>1 
    and pa.adj_void_ind<>1
    and pa.adj_rrsn_ind<>1
    and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV'
	and sd.coml_agmt_id=@coml_agmt_id
	and sd.st_id=@st_id
	and sd.ln_of_bsn_id=@ln_of_bsn_id
	and sd.surchrg_cd_id=@surchrg_cd_id
	and sd.surchrg_typ_id=@surchrg_typ_id
    
   return @cnt_prev_adjs

end


go

if object_id('fn_DetermineFirstAdjustment_Surcharge') is not null
	print 'Created function fn_DetermineFirstAdjustment_Surcharge'
else
	print 'Failed Creating function fn_DetermineFirstAdjustment_Surcharge'
go

if object_id('fn_DetermineFirstAdjustment_Surcharge') is not null
	grant exec on fn_DetermineFirstAdjustment_Surcharge to public
go
