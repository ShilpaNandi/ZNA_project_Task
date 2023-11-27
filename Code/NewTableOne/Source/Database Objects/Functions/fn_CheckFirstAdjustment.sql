
if exists (select 1 from sysobjects 
		where name = 'fn_CheckFirstAdjustment' and type = 'FN')
	drop function fn_CheckFirstAdjustment
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_CheckFirstAdjustment
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Naresh Kumar
-----
-----	Description:	This function  checks whether this is first adjustment for NON-PREMIUM adjustment types.
-----
-----					Filetring Based on revision,void and cancel indicators
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_CheckFirstAdjustment]
   (
		@premium_adj_prog_id int
	)
returns int
as
begin
	declare @count int
	set @count = 0

	/**************************
	* Check first adjustment
	**************************/
    select @count= count(distinct pa.prem_adj_id) from dbo.PREM_ADJ pa
	inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.reg_custmr_id = ps.custmr_id)
	inner join dbo.PREM_ADJ_PERD prd on (pa.reg_custmr_id = prd.reg_custmr_id) and (pa.prem_adj_id = prd.prem_adj_id)
	where ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
	and prd.prem_adj_pgm_id = @premium_adj_prog_id
    and pa.adj_can_ind<>1 
    and pa.adj_void_ind<>1
    and pa.adj_rrsn_ind<>1
    and substring(pa.fnl_invc_nbr_txt,1,3)<>'RTV'
    
   return @count
end


go

if object_id('fn_CheckFirstAdjustment') is not null
	print 'Created function fn_CheckFirstAdjustment'
else
	print 'Failed Creating function fn_CheckFirstAdjustment'
go

if object_id('fn_CheckFirstAdjustment') is not null
	grant exec on fn_CheckFirstAdjustment to public
go


