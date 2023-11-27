
if exists (select 1 from sysobjects 
		where name = 'PrgAISCalcResults' and type = 'P')
	drop procedure PrgAISCalcResults
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	PrgAISCalcResults
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used for delete records from tables storing calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[PrgAISCalcResults] 
@customer_id int,
@premium_adjustment_id int,
@premium_adj_period_id int,
@delete_plb bit,
@delete_ilrf bit
as

begin
	set nocount on

declare	@trancount int


set @trancount = @@trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	delete from PREM_ADJ_PARMET_DTL WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_ARIES_CLRING WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	--and prem_adj_perd_id = @premium_adj_period_id

--	delete from dbo.PREM_ADJ_COMB_ELEMTS WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id
	if(@delete_ilrf=1)
	begin
	delete from dbo.PREM_ADJ_LOS_REIM_FUND_POST WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id
	end
	
	if (@delete_plb = 1)
	begin
		delete from dbo.PREM_ADJ_PAID_LOS_BIL WITH (ROWLOCK)
		where custmr_id = @customer_id
		and prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
	end

--	delete from dbo.PREM_ADJ_MISC_INVC WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_CMMNT WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

--	delete from dbo.PREM_ADJ_NY_SCND_INJR_FUND WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_PERD_TOT WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id

	delete from dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id



	--clear Premium Adjustment Period table after clearing results output table
	-- This delete operation being performed in the driver
--	delete from dbo.PREM_ADJ_PERD WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id
--	and prem_adj_perd_id = @premium_adj_period_id

	--clear Premium Adjustment Status table after clearing results output table
--	delete from dbo.PREM_ADJ_STS WITH (ROWLOCK)
--	where custmr_id = @customer_id
--	and prem_adj_id = @premium_adjustment_id

	--clear Premium Adjustment table after clearing results output table
--	if not exists(select * from dbo.PREM_ADJ_PERD where custmr_id = @customer_id and prem_adj_id = @premium_adjustment_id )
--	begin
--		delete from dbo.PREM_ADJ WITH (ROWLOCK)
--		where reg_custmr_id = @customer_id
--		and prem_adj_id = @premium_adjustment_id
--	end


	if @trancount = 0
		commit transaction

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end

	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no,'') + '; procedure:' 
		+ isnull(@err_proc,'') + ';error line:' + isnull(@err_ln,'') + ';description:' + isnull(@err_msg,'') 

	insert into [dbo].[APLCTN_STS_LOG]
   (
		[src_txt]
	   ,[sev_cd]
	   ,[shrt_desc_txt]
	   ,[full_desc_txt]
	   ,[custmr_id]
	   ,[prem_adj_id]
	   ,[crte_user_id]
	)
     values
    (
		'AIS Calculation Engine'
       ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for adjustment period number: ' 
			+ convert(varchar(20),isnull(@premium_adj_period_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ @err_msg
	   ,@customer_id
	   ,@premium_adjustment_id
       ,1
	)

	select 
    error_number() AS ErrorNumber,
    error_severity() AS ErrorSeverity,
    error_state() as ErrorState,
    error_procedure() as ErrorProcedure,
    error_line() as ErrorLine,
    error_message() as ErrorMessage
	
	declare @err_sev varchar(10)

	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line(),
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )

end catch


end



go

if object_id('PrgAISCalcResults') is not null
	print 'Created Procedure PrgAISCalcResults'
else
	print 'Failed Creating Procedure PrgAISCalcResults'
go

if object_id('PrgAISCalcResults') is not null
	grant exec on PrgAISCalcResults to public
go







