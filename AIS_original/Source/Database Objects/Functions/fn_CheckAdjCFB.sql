
if exists (select 1 from sysobjects 
		where name = 'fn_CheckAdjCFB' and type = 'FN')
	drop function fn_CheckAdjCFB
GO
set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_CheckAdjCFB
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Mogali
-----
-----	Description:	Check the adjCFB or CFB in Paid and Incurred ERP calculation, when Min Max applies.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_CheckAdjCFB]
   (
	@AdjId int,
    @PGMId int,
	@PerdId int	
	)
returns int
as
begin
    declare @return int
	declare @minimum decimal(15,2)
	declare @maximum decimal(15,2)
	declare @IncERP decimal(15,2)
	declare @PaidERP decimal(15,2)
			
	SELECT @minimum = case when isnull(tot_agmt_amt,0) > isnull(tot_audt_amt,0) 
	then isnull(tot_agmt_amt,0) else isnull(tot_audt_amt,0) end 
	FROM PREM_ADJ_PERD
	INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.ACTV_IND = 1 
	AND PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
	INNER JOIN PREM_ADJ_PGM_STS ON PREM_ADJ_PGM_STS.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
	AND PREM_ADJ_PGM_STS.PGM_PERD_STS_TYP_ID = 342 AND PREM_ADJ_PGM_STS.STS_CHK_IND = 1
	INNER JOIN PREM_ADJ_PGM_RETRO ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.PREM_ADJ_PGM_ID
	WHERE PREM_ADJ_PERD.PREM_ADJ_ID =  @AdjId AND 
	PREM_ADJ_PGM_RETRO.RETRO_ELEMT_TYP_ID = 338  AND PREM_ADJ_PERD.PREM_ADJ_PGM_ID = @PGMId

	SELECT @maximum = case when isnull(tot_agmt_amt,0) > isnull(tot_audt_amt,0) 
	then isnull(tot_agmt_amt,0) else isnull(tot_audt_amt,0) end 
	FROM PREM_ADJ_PGM_RETRO INNER JOIN PREM_ADJ_PERD 
	ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.PREM_ADJ_PGM_ID
	AND PREM_ADJ_PERD.CUSTMR_ID = PREM_ADJ_PGM_RETRO.CUSTMR_ID 
	INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.ACTV_IND = 1 
	AND PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
	INNER JOIN PREM_ADJ_PGM_STS ON PREM_ADJ_PGM_STS.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
	AND PREM_ADJ_PGM_STS.PGM_PERD_STS_TYP_ID = 342 AND PREM_ADJ_PGM_STS.STS_CHK_IND = 1
	WHERE PREM_ADJ_PERD.PREM_ADJ_ID =  @AdjId AND 
	PREM_ADJ_PGM_RETRO.RETRO_ELEMT_TYP_ID = 337 AND PREM_ADJ_PERD.PREM_ADJ_PGM_ID = @PGMId

	if (			
			((select no_lim_ind 
			from dbo.PREM_ADJ_PGM_RETRO
			where prem_adj_pgm_id = @PGMId
			and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
			and actv_ind = 1) = 1)
			or
			((select retro_adj_fctr_aplcbl_ind 
			from dbo.PREM_ADJ_PGM_RETRO
			where prem_adj_pgm_id = @PGMId
			and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
			and actv_ind = 1) = 1)
			
		)
		begin
			set @maximum = 1000000000000					
		end

	if (			
			((select no_lim_ind 
			from dbo.PREM_ADJ_PGM_RETRO
			where prem_adj_pgm_id = @PGMId
			and retro_elemt_typ_id = 338 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Miniimum
			and actv_ind = 1) = 1)
			or
			((select retro_adj_fctr_aplcbl_ind 
			from dbo.PREM_ADJ_PGM_RETRO
			where prem_adj_pgm_id = @PGMId
			and retro_elemt_typ_id = 338 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Miniimum
			and actv_ind = 1) = 1)
		)
		begin
			set @minimum = 0 --@minmax_incurred_erp_by_pp_amt
		end

	select @IncERP = sum(isnull(adj_incur_ernd_retro_prem_amt,0)) from prem_adj_retro_dtl 
	where prem_adj_id = @AdjId and prem_adj_pgm_id = @PGMId and prem_adj_perd_id = @PerdId

	select @PaidERP = sum(isnull(adj_incur_ernd_retro_prem_amt,0)) - sum(isnull(cash_flw_ben_amt,0)) 
	from prem_adj_retro_dtl where prem_adj_id = @AdjId and prem_adj_pgm_id = @PGMId and prem_adj_perd_id = @PerdId

	select @return = case when (@IncERP >= @minimum and @PaidERP <= @minimum) then 1 
						  when (@IncERP >= @maximum and @PaidERP <= @maximum) then 1 
						  when (@IncERP >= @maximum and @PaidERP >= @maximum) then 1 
						  when (@IncERP <= @minimum and @PaidERP <= @minimum) then 1 
						  when (@IncERP > @maximum and @PaidERP > @maximum) then 1 
						  when (@IncERP < @minimum and @PaidERP < @minimum) then 1 
						  else 0 end

	return @return 
end


go

if object_id('fn_CheckAdjCFB') is not null
	print 'Created function fn_CheckAdjCFB'
else
	print 'Failed Creating function fn_CheckAdjCFB'
go

if object_id('fn_CheckAdjCFB') is not null
	grant exec on fn_CheckAdjCFB to public
go




