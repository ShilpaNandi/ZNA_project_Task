
if exists (select 1 from sysobjects 
		where name = 'fn_ComputeSubAudPremiumRatio' and type = 'FN')
	drop function fn_ComputeSubAudPremiumRatio
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ComputeSubAudPremiumRatio
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  computes the ratio by which amounts at program
-----					period level need to be split proportionately.
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----	Modified:	Venkat Kolimi
-----			- Fixed the TFS Bug 12896 to get the correct percentage of the basic for the given state.


---------------------------------------------------------------------
create function [dbo].[fn_ComputeSubAudPremiumRatio]
   (
		@customer_id int,
		@premium_adj_prog_id int,
		@coml_agmt_id int,
		@state_id int
	)
returns decimal(15,8)
as
begin
	declare @prem_amount decimal(15,2),
			@sum_prem_amount decimal(15,2)


	--------------------------------------------------
	    --Bug Fix 12896
	--------------------------------------------------
	select  
	@prem_amount =isnull(spa.prem_amt,0)
	from dbo.COML_AGMT ca
	inner join dbo.COML_AGMT_AUDT caa on (ca.coml_agmt_id = caa.coml_agmt_id) and (ca.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (ca.custmr_id = caa.custmr_id)
	inner join dbo.SUBJ_PREM_AUDT spa on (spa.coml_agmt_audt_id = caa.coml_agmt_audt_id) and (spa.coml_agmt_id = caa.coml_agmt_id) and (spa.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (spa.custmr_id = caa.custmr_id)
	where ca.prem_adj_pgm_id = @premium_adj_prog_id
	and ca.custmr_id = @customer_id
	and ca.actv_ind = 1
	and spa.actv_ind = 1
	and spa.coml_agmt_id = @coml_agmt_id
	and spa.st_id = @state_id
	and (caa.audt_revd_sts_ind = 0 or  caa.audt_revd_sts_ind is null)
	and ca.adj_typ_id not in (62,64,68,69,448)

	set @prem_amount = isnull(@prem_amount,0)

	select  
	@sum_prem_amount = sum(isnull(spa.prem_amt,0))
	from dbo.COML_AGMT ca
	inner join dbo.COML_AGMT_AUDT caa on (ca.coml_agmt_id = caa.coml_agmt_id) and (ca.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (ca.custmr_id = caa.custmr_id)
	inner join dbo.SUBJ_PREM_AUDT spa on (spa.coml_agmt_audt_id = caa.coml_agmt_audt_id) and (spa.coml_agmt_id = caa.coml_agmt_id) and (spa.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (spa.custmr_id = caa.custmr_id)
	where ca.prem_adj_pgm_id = @premium_adj_prog_id
	and ca.custmr_id = @customer_id
	and ca.actv_ind = 1
	and spa.actv_ind = 1
	and (caa.audt_revd_sts_ind = 0 or  caa.audt_revd_sts_ind is null)
	and ca.adj_typ_id not in (62,64,68,69,448)

	set @sum_prem_amount = isnull(@sum_prem_amount,0)

--	select 
--	@sum_prem_amount = sum(prem_amt)
--	from dbo.SUBJ_PREM_AUDT
--	where custmr_id = @customer_id
--	and prem_adj_pgm_id = @premium_adj_prog_id

   return case when @sum_prem_amount <> 0 then (@prem_amount / @sum_prem_amount)*100 else 0 end
end


go

if object_id('fn_ComputeSubAudPremiumRatio') is not null
	print 'Created function fn_ComputeSubAudPremiumRatio'
else
	print 'Failed Creating function fn_ComputeSubAudPremiumRatio'
go

if object_id('fn_ComputeSubAudPremiumRatio') is not null
	grant exec on fn_ComputeSubAudPremiumRatio to public
go


