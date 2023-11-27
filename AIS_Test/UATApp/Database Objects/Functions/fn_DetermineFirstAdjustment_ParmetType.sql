
if exists (select 1 from sysobjects 
		where name = 'fn_DetermineFirstAdjustment_ParmetType' and type = 'FN')
	drop function fn_DetermineFirstAdjustment_ParmetType
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_DetermineFirstAdjustment_ParmetType
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  determines whether this is first adjustment for NON-PREMIUM adjustment types.
-----
-----					03/12/2009 venkat kolimi 
-----					Filetring Based on revision,void and cancel indicators
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_DetermineFirstAdjustment_ParmetType]
   (
		@premium_adj_prog_id int,
		@adj_parmet_typ_id int

	)
returns smallint
as
begin
	declare @cnt_prev_adjs smallint

	/**************************
	* Determine first adjustment
	**************************/

	select @cnt_prev_adjs = count(distinct pa.prem_adj_id) from dbo.PREM_ADJ pa
	inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.reg_custmr_id = ps.custmr_id)
	inner join dbo.PREM_ADJ_PERD prd on (pa.reg_custmr_id = prd.reg_custmr_id) and (pa.prem_adj_id = prd.prem_adj_id)
	inner join dbo.PREM_ADJ_PARMET_SETUP op on /* (pa.reg_custmr_id = op.custmr_id) and */ (pa.prem_adj_id = op.prem_adj_id )
	where ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
	and prd.prem_adj_pgm_id = @premium_adj_prog_id
  	and op.adj_parmet_typ_id = @adj_parmet_typ_id
    and pa.adj_can_ind<>1 
    and pa.adj_void_ind<>1
    and pa.adj_rrsn_ind<>1
    and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV'
    
   return @cnt_prev_adjs
end


go

if object_id('fn_DetermineFirstAdjustment_ParmetType') is not null
	print 'Created function fn_DetermineFirstAdjustment_ParmetType'
else
	print 'Failed Creating function fn_DetermineFirstAdjustment_ParmetType'
go

if object_id('fn_DetermineFirstAdjustment_ParmetType') is not null
	grant exec on fn_DetermineFirstAdjustment_ParmetType to public
go


