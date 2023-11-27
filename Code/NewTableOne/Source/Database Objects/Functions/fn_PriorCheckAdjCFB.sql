
if exists (select 1 from sysobjects 
		where name = 'fn_PriorCheckAdjCFB' and type = 'FN')
	drop function fn_PriorCheckAdjCFB
GO
set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_PriorCheckAdjCFB
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
create function [dbo].[fn_PriorCheckAdjCFB]
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
			
	SELECT @minimum = ernd_retro_prem_min_amt
	FROM PREM_ADJ_PERD	
	WHERE PREM_ADJ_ID =  @AdjId AND PREM_ADJ_PGM_ID = @PGMId AND PREM_ADJ_PERD_ID = @PerdId

	SELECT @maximum = ernd_retro_prem_max_amt
	FROM PREM_ADJ_PERD	
	WHERE PREM_ADJ_ID =  @AdjId AND PREM_ADJ_PGM_ID = @PGMId AND PREM_ADJ_PERD_ID = @PerdId

	if (			
			((SELECT ernd_retro_prem_unlim_ind
			FROM PREM_ADJ_PERD	
			WHERE PREM_ADJ_ID =  @AdjId AND PREM_ADJ_PGM_ID = @PGMId AND PREM_ADJ_PERD_ID = @PerdId) = 1)			
		)
		begin
			set @maximum = 1000000000000					
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

if object_id('fn_PriorCheckAdjCFB') is not null
	print 'Created function fn_PriorCheckAdjCFB'
else
	print 'Failed Creating function fn_PriorCheckAdjCFB'
go

if object_id('fn_PriorCheckAdjCFB') is not null
	grant exec on fn_PriorCheckAdjCFB to public
go




